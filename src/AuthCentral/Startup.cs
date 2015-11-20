using System;

using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

using Fsw.Enterprise.AuthCentral.Health;
using Fsw.Enterprise.AuthCentral.IdMgr;

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
            services.AddAuthCentralDependencies(_config);
            services.AddAuthorizationPolicies();
            services.AddIdentityServer(_config.DB.IdentityServer3);
        }

        public void Configure(IApplicationBuilder app, IApplicationEnvironment env, ILoggerFactory logFactory, UserAccountService<HierarchicalUserAccount> userAccountService)
        {
            MembershipRebootSetup.GetConfig(app); // Create the singleton to get around MVC DI container limitations
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
            
            app.UseIdentityServer(_config);

            app.UseMvc();
        }
    }
}
