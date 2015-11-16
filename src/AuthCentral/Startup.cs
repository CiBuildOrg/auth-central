using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BrockAllen.MembershipReboot;

using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Authentication;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr;
using Fsw.Enterprise.AuthCentral.MongoDb;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

using IdentityServer3.Core.Services;
using IdentityServer3.Core.Configuration;
using AuthenticationOptions = IdentityServer3.Core.Configuration.AuthenticationOptions;

using MongoDB.Driver;
using IdentityServer3.MembershipReboot;
using MongoDatabase = Fsw.Enterprise.AuthCentral.MongoDb.MongoDatabase;

using Serilog;

using Fsw.Enterprise.AuthCentral.MongoStore.Admin;
using Fsw.Enterprise.AuthCentral.Health;
using Fsw.Enterprise.AuthCentral.IdSvr;
using Fsw.Enterprise.AuthCentral.MongoStore;

namespace Fsw.Enterprise.AuthCentral
{
    public class Startup
    {
        private EnvConfig _config;
        private IdentityServerServiceFactory _idSvcfactory;
        private StoreSettings _idSvrStoreSettings = StoreSettings.DefaultSettings();

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("appsettings.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            _config = new EnvConfig(Configuration);

            _idSvrStoreSettings.ConnectionString = _config.DB.IdentityServer3;
            _idSvrStoreSettings.Database = MongoUrl.Create(_idSvrStoreSettings.ConnectionString).DatabaseName;

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
            services.AddAuthentication( sharedOptions => sharedOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
            services.AddScoped<MembershipRebootConfiguration<HierarchicalUserAccount>>(provider => MembershipRebootSetup.GetConfig(null));
            services.AddScoped<UserAccountService<HierarchicalUserAccount>>();
            services.AddScoped(typeof (IUserAccountRepository<HierarchicalUserAccount>), typeof (MongoUserAccountRepository<HierarchicalUserAccount>));
            services.AddScoped(provider => new MongoDatabase(_config.DB.MembershipReboot));
            services.AddScoped<MongoAuthenticationService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("FswPlatform", policy => {
                    policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
                    policy.RequireClaim("scope", "fsw_platform");
                });

                options.AddPolicy("FswAdmin", policy => {
                    policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
                    policy.RequireClaim("scope", "fsw_platform");
                    policy.RequireClaim("fsw:authcentral:admin", "true");
                });
                
                options.DefaultPolicy = options.GetPolicy("FswPlatform");
            });
            services.AddInstance<EnvConfig>(_config);
            services.AddInstance<IClientService>(AdminServiceFactory.CreateClientService(_idSvrStoreSettings));
            services.AddInstance<IScopeService>(AdminServiceFactory.CreateScopeService(_idSvrStoreSettings));
        }

        public void Configure(IApplicationBuilder app, IApplicationEnvironment env, ILoggerFactory logFactory)
        {
            // TODO: This whole method should be refactored
            MembershipRebootSetup.GetConfig(app); // Create the singleton to get around MVC DI container limitations
            var settings = StoreSettings.DefaultSettings();

            settings.ConnectionString = _config.DB.IdentityServer3;
            settings.Database = MongoUrl.Create(settings.ConnectionString).DatabaseName;

            var usrSrv = new Registration<IUserService, MembershipRebootUserService<HierarchicalUserAccount>>();
            _idSvcfactory = new ServiceFactory(usrSrv, _idSvrStoreSettings)
            {
                ViewService = new Registration<IViewService>(typeof(CustomViewService))
            };

            _idSvcfactory.ConfigureCustomUserService(app, _config.DB.MembershipReboot);
            _idSvcfactory.Register(new Registration<IApplicationEnvironment>(env));
            
            app.UseDeveloperExceptionPage();

            app.UseCookieAuthentication(options =>
            {
                options.LoginPath = new PathString("/ids/login");
                options.AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });

            app.UseOpenIdConnectAuthentication(options =>
            {
                options.AuthenticationScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ClientId = "auth_central_client";
                options.ClientSecret = "secret";
                options.Authority = new UriBuilder(_config.Uri.Scheme, _config.Uri.Host, _config.Uri.Port, "ids").Uri.AbsoluteUri;
                options.RedirectUri = new UriBuilder(_config.Uri.Scheme, _config.Uri.Host, _config.Uri.Port, "account").Uri.AbsoluteUri;
                options.ResponseType = OpenIdConnectResponseTypes.Code;
                options.DefaultToCurrentUriOnRedirect = true;
                options.Scope.Add("fsw_platform");
                options.Scope.Add("openid");

                options.Events = new OpenIdConnectEvents
                {
                    OnAuthenticationValidated = data =>
                    {
                        var id = new ClaimsIdentity("application", "given_name", "role");

                        var token = new JwtSecurityToken(data.TokenEndpointResponse.ProtocolMessage.AccessToken);
                        IEnumerable<Claim> claims = token.Claims.Where(c => c.Type != "iss" &&
                                                                            c.Type != "aud" &&
                                                                            c.Type != "nbf" &&
                                                                            c.Type != "exp" &&
                                                                            c.Type != "iat" &&
                                                                            c.Type != "nonce" &&
                                                                            c.Type != "c_hash" &&
                                                                            c.Type != "at_hash");

                        string expiration = token.Claims.First(c => c.Type == "exp").Value;

                        id.AddClaims(claims);
                        id.AddClaim(new Claim("expires_at",
                            new DateTime(1970, 1, 1).AddSeconds(Convert.ToDouble(expiration))
                                .ToString(CultureInfo.CurrentCulture)));

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
                app.UseDeveloperExceptionPage();
            } else
            {
                logFactory.MinimumLevel = LogLevel.Error;
            }

            app.UseIISPlatformHandler();
            app.UseStaticFiles();

            logFactory.AddSerilog();
            HealthChecker.ScheduleHealthCheck(_config, logFactory);

            app.Map("/ids", ids =>
            {
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
                    Factory = _idSvcfactory,
                    AuthenticationOptions = new AuthenticationOptions()
                    {
                        EnableLocalLogin = true,
                        EnableLoginHint = true,
                        RememberLastUsername = false,
                        CookieOptions = new IdentityServer3.Core.Configuration.CookieOptions()
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRouteWithClientId",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{clientId?}"
                );
            });
        }
    }
}
