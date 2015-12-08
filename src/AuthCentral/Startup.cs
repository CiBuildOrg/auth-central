using System;

using cloudscribe.Web.Pagination;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions; // Yes, really.
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;

using Fsw.Enterprise.AuthCentral.Extensions;
using Fsw.Enterprise.AuthCentral.Health;
using Fsw.Enterprise.AuthCentral.IdMgr;
using Fsw.Enterprise.AuthCentral.MongoStore;

namespace Fsw.Enterprise.AuthCentral
{
    public class Startup
    {
        private EnvConfig _config;
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            _config = new EnvConfig(Configuration);
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSerilog(_config.IsDebug);
            services.AddDataProtection();
            services.AddMvc();
            services.AddMembershipReboot(_config.DB.MembershipReboot);
            services.AddAuthorizationPolicies();
            services.AddAuthCentralDependencies(_config);
            services.AddIdentityServer(_config.DB.IdentityServer3);
            services.TryAddTransient<IBuildPaginationLinks, PaginationLinkBuilder>();
        }

        public void Configure(IApplicationBuilder app, IApplicationEnvironment env, ILoggerFactory logFactory, StoreSettings idSvrStoreSettings)
        {
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

            app.Map("/ids", ids =>
            {
                ids.UseIdentityServer(env, _config, idSvrStoreSettings);
            });
            
            app.UseMvc();
        }
    }
}