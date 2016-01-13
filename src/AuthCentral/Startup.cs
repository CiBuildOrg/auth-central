using System;

using cloudscribe.Web.Pagination;
using Microsoft.AspNet.Authentication.Cookies;

using Fsw.Enterprise.AuthCentral.Extensions;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions; // Yes, really.
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNet.Http;
using Fsw.Enterprise.AuthCentral.IdMgr;
using Fsw.Enterprise.AuthCentral.Health;
using Fsw.Enterprise.AuthCentral.MongoStore;

namespace Fsw.Enterprise.AuthCentral
{
    public class Startup
    {
        private EnvConfig _config;

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true);
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
            services.TryAddTransient<IBuildPaginationLinks, PaginationLinkBuilder>();
        }

        public void Configure(IApplicationBuilder app, IApplicationEnvironment env, ILoggerFactory logFactory, StoreSettings idSvrStoreSettings)
        {
            app.ConfigureLoggers(logFactory, _config.IsDebug);
            logFactory.AddSerilog();
            logFactory.AddProvider(new LogCentral.MicrosoftFramework.LoggingProvider(_config.Log4NetConfigPath));
            app.UseMiddleware<LogCentral.MicrosoftFramework.LogMiddleware>();

            MembershipRebootSetup.GetConfig(app, env); // Create the singleton to get around MVC DI container limitations
            app.UseStatusCodePagesWithReExecute("/errors/{0}.html");
            app.UseCookieAuthentication(options =>
            {
                options.LoginPath = new PathString(_config.Uri.LoginPath);
                options.AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });

            app.UseOpenIdConnectAuthentication(_config);
            app.UseIISPlatformHandler();
            app.UseStaticFiles();
            HealthChecker.ScheduleHealthCheck(_config, logFactory);

            app.Map(_config.Uri.AuthorityMapPath, ids =>
            {
                ids.UseIdentityServer(env, _config, idSvrStoreSettings);
            });
            
            app.UseMvc();
        }
    }
}
