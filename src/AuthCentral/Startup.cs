using Fsw.Enterprise.AuthCentral.IdSvr;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Logging;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Owin;
using Serilog;
using System;

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
 