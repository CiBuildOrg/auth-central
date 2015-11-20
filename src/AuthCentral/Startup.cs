using System;

using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

using BrockAllen.MembershipReboot.Hierarchical;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.MembershipReboot;

using Fsw.Enterprise.AuthCentral.Health;
using Fsw.Enterprise.AuthCentral.IdMgr;
using Fsw.Enterprise.AuthCentral.IdSvr;
using Fsw.Enterprise.AuthCentral.MongoStore;

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
            services.AddSerilog(_config.IsDebug);
            services.AddDataProtection();
            services.AddMvc();
            services.AddMembershipReboot(_config.DB.MembershipReboot);
            services.AddAuthorizationPolicies();
            services.AddAuthCentralDependencies(_config);
            services.AddIdentityServer(_config.DB.IdentityServer3);
        }

        public void Configure(IApplicationBuilder app, IApplicationEnvironment env, ILoggerFactory logFactory, StoreSettings idSvrStoreSettings)
        {
            // TODO: This whole method should be refactored
            MembershipRebootSetup.GetConfig(app); // Create the singleton to get around MVC DI container limitations
            
            app.UseStatusCodePages();
            app.UseCookieAuthentication(options =>
            {
                options.LoginPath = new PathString("/ids/login");
                options.AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });

            app.UseOpenIdConnectAuthentication(_config);
            app.ConfigureLoggers(logFactory, _config.IsDebug);
            app.UseIISPlatformHandler();
            app.UseStaticFiles();
            logFactory.AddSerilog();
            HealthChecker.ScheduleHealthCheck(_config, logFactory);

            var usrSrv = new Registration<IUserService, MembershipRebootUserService<HierarchicalUserAccount>>();
            var idSvcFactory = new ServiceFactory(usrSrv, idSvrStoreSettings)
            {
                ViewService = new Registration<IViewService>(typeof(CustomViewService))
            };

            idSvcFactory.ConfigureCustomUserService(app, _config.DB.MembershipReboot);
            idSvcFactory.Register(new Registration<IApplicationEnvironment>(env));

            app.Map("/ids", ids =>
            {

                var idsOptions = new IdentityServerOptions
                {
                    SiteName = "FSW Identity Server",
                    PublicOrigin = _config.Uri.IssuerUri,
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
                    Factory = idSvcFactory,
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

            app.UseMvc();
        }
    }
}