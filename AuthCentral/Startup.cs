using Fsw.Enterprise.AuthCentral.IdMgr;
using Fsw.Enterprise.AuthCentral.IdSvr;
using IdentityManager.Configuration;
using IdentityManager.Core.Logging;
using IdentityManager.Extensions;
using IdentityManager.Logging;
using IdentityServer3.Core.Configuration;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.OpenIdConnect;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Owin;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral
{
    public class Startup
    {

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("appsettings.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IApplicationEnvironment env)
        {
            var config = new EnvConfig(Configuration);

            app.UseIISPlatformHandler();
            app.UseDeveloperExceptionPage();
            LogProvider.SetCurrentLogProvider(new TraceSourceLogProvider());
            
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies"
            });

            var oidcOptions = new OpenIdConnectOptions("Cookies")
            {
                DisplayName = null, // this will hide the openidconnect button on the login page
                // AuthenticationType = "oidc",
                Authority = new UriBuilder(config.Uri.Scheme, config.Uri.Host, config.Uri.Port, "ids").Uri.AbsoluteUri,
                ClientId = "idmgr_client",
                RedirectUri = config.Uri.IssuerUri,
                ResponseType = "id_token",
                UseTokenLifetime = false,
                Events = new OpenIdConnectEvents()
                {
                    OnAuthenticationValidated = n =>
                    {
                        // n.AuthenticationTicket.Properties.
                        return Task.FromResult(0);
                    }
                },

                //Notifications = new Microsoft.Owin.Security.OpenIdConnect.OpenIdConnectAuthenticationNotifications
                //{
                //    SecurityTokenValidated = n =>
                //    {
                //        n.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
                //        return Task.FromResult(0);
                //    },
                //    RedirectToIdentityProvider = async n =>
                //    {
                //        if (n.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType.LogoutRequest)
                //        {
                //            var result = await n.OwinContext.Authentication.AuthenticateAsync("Cookies");
                //            if (result != null)
                //            {
                //                var id_token = result.Identity.Claims.GetValue("id_token");
                //                if (id_token != null)
                //                {
                //                    n.ProtocolMessage.IdTokenHint = id_token;
                //                    n.ProtocolMessage.PostLogoutRedirectUri = new UriBuilder(Program.EnvConfig.Uri.Scheme, Program.EnvConfig.Uri.Host, Program.EnvConfig.Uri.Port, "idm").Uri.AbsoluteUri;
                //                }
                //            }
                //        }
                //    }
                //}
            };

            oidcOptions.Scope.Add("idmgr");
            app.UseOpenIdConnectAuthentication(oidcOptions);
            
            app.Map("/idm", idm =>
            {
                LogProvider.SetCurrentLogProvider(new DiagnosticsTraceLogProvider());

                var factory = new IdentityManagerServiceFactory();
                factory.Configure(app, config.DB.MembershipReboot);

                idm.UseIdentityManager(new IdentityManagerOptions
                {
                    Factory = factory,
                    SecurityConfiguration = new HostSecurityConfiguration
                    {
                        HostAuthenticationType = "Cookies",
                    }

                });
            });

            app.Map("/ids", ids =>
            {
                Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Verbose()
                                .WriteTo.Trace()
                                .CreateLogger();

                var idSvrFactory = Factory.Configure(config.DB.IdentityServer3);
                idSvrFactory.ConfigureCustomUserService(app, config.DB.MembershipReboot);

                var idsOptions = new IdentityServerOptions
                {
                    SiteName = "FSW Identity Server",
                    SigningCertificate = Certificate.Get(config.Cert.StoreName, config.Cert.Thumbprint),
                    IssuerUri = config.Uri.IssuerUri,
                    RequireSsl = true,
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
 