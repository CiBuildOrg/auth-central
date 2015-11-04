using Fsw.Enterprise.AuthCentral.Health;
using Fsw.Enterprise.AuthCentral.IdSvr;
using IdentityServer3.Core.Configuration;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using AuthenticationOptions = IdentityServer3.Core.Configuration.AuthenticationOptions;

namespace Fsw.Enterprise.AuthCentral
{
    public class Startup
    {
        private EnvConfig _config;
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("appsettings.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            _config = new EnvConfig(Configuration);
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            var loggerConfig = new LoggerConfiguration()
               .WriteTo.Trace()
               .WriteTo.Console();

            if (_config.IsDebug)
            {
                loggerConfig.MinimumLevel.Verbose();
            } else
            {
                loggerConfig.MinimumLevel.Error();
            }

            Log.Logger = loggerConfig.CreateLogger();
            
            services.AddDataProtection();
            services.AddMvc();
            services.AddAuthentication(
                sharedOptions => sharedOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public void Configure(IApplicationBuilder app, IApplicationEnvironment env, ILoggerFactory logFactory)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();

            app.UseCookieAuthentication(options =>
            {
                options.AutomaticAuthentication = true;
                options.AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });

            app.UseOpenIdConnectAuthentication(options =>
            {
                options.AutomaticAuthentication = true;
                options.AuthenticationScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ClientId = "auth_central_client";
                options.ClientSecret = "secret";
                options.Authority = new UriBuilder(_config.Uri.Scheme, _config.Uri.Host, _config.Uri.Port, "ids").Uri.AbsoluteUri;
                options.RedirectUri = new UriBuilder(_config.Uri.Scheme, _config.Uri.Host, _config.Uri.Port, "account").Uri.AbsoluteUri;
                options.ResponseType = OpenIdConnectResponseTypes.Code;
                options.DefaultToCurrentUriOnRedirect = true;
                options.Scope.Add("fsw_platform");

                options.Events = new OpenIdConnectEvents
                {
                    OnAuthenticationValidated = data =>
                    {
                        var incoming = data.AuthenticationTicket.Principal;
                        var id = new ClaimsIdentity("application", "given_name", "role");

                        id.AddClaim(incoming.FindFirst("sub"));
                        id.AddClaim(new Claim("access_token", data.TokenEndpointResponse.ProtocolMessage.AccessToken));
                        id.AddClaim(new Claim("id_token", data.TokenEndpointResponse.ProtocolMessage.IdToken));
                        id.AddClaim(new Claim("expires_at",
                            DateTime.Now.AddSeconds(double.Parse(data.TokenEndpointResponse.ProtocolMessage.ExpiresIn))
                                .ToString(CultureInfo.InvariantCulture)));

                        data.AuthenticationTicket = new AuthenticationTicket(
                            new ClaimsPrincipal(id),
                            data.AuthenticationTicket.Properties,
                            data.AuthenticationTicket.AuthenticationScheme);

                        return Task.FromResult(0);
                    }
                };
            });

            if(_config.IsDebug)
            {
                logFactory.MinimumLevel = LogLevel.Verbose;
            } else
            {
                logFactory.MinimumLevel = LogLevel.Error;
            }
            app.UseIISPlatformHandler();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

            logFactory.AddSerilog();
            HealthChecker.ScheduleHealthCheck(_config, logFactory);
            
            app.Map("/ids", ids =>
            {
                var idSvrFactory = Factory.Configure(_config.DB.IdentityServer3);
                idSvrFactory.ConfigureCustomUserService(app, _config.DB.MembershipReboot);
                idSvrFactory.Register(new Registration<IApplicationEnvironment>(env));

                var idsOptions = new IdentityServerOptions
                {
                    SiteName = "FSW Identity Server",
                    SigningCertificate = Certificate.Get(_config.Cert.StoreName, _config.Cert.Thumbprint),
                    IssuerUri = _config.Uri.IssuerUri,
                    RequireSsl = true,
                    LoggingOptions = new LoggingOptions()
                    {
                        EnableHttpLogging = true,
                        EnableKatanaLogging = _config.IsDebug,
                        EnableWebApiDiagnostics = _config.IsDebug,
                        WebApiDiagnosticsIsVerbose = _config.IsDebug
                    },
                    Endpoints = new EndpointOptions()
                    {
                        EnableCspReportEndpoint = true
                    },
                    Factory = idSvrFactory,
                    AuthenticationOptions = new AuthenticationOptions()
                    {
                        EnableLocalLogin = true,
                        EnableLoginHint = true,
                        RememberLastUsername = false,
                        CookieOptions = new CookieOptions()
                        {
                            ExpireTimeSpan = new TimeSpan(10, 0, 0),
                            IsPersistent = false,
                            SlidingExpiration = false,
                            AllowRememberMe = true,
                            RememberMeDuration = new TimeSpan(30, 0, 0, 0)
                        },
                        EnableSignOutPrompt = true,
                        EnablePostSignOutAutoRedirect = true,
                        SignInMessageThreshold = 5
                    },
                    CspOptions = new CspOptions()
                    {
                        Enabled = true
                    },
                    EnableWelcomePage = true
                };

                ids.UseIdentityServer(idsOptions);
            });

            app.UseMvc();
        }
    }
}
