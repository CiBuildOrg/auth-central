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
using Fsw.Enterprise.AuthCentral.MongoStore;
using Fsw.Enterprise.AuthCentral.IdMgr;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.MongoStore.Admin;
using BrockAllen.MembershipReboot;
using Fsw.Enterprise.AuthCentral.Crypto;

namespace Fsw.Enterprise.AuthCentral
{
    public class Startup
    {
        private EnvConfig _config;
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettingdefaults.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            _config = new EnvConfig(Configuration);

            // BUGBUG: This could be a problem, depending on the order things get loaded
            AuthCentralDataProtectionStartup.AuthCentralEnvConfig = _config;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSerilog(_config.IsDebug);
            services.AddDataProtection();
            services.ConfigureDataProtection(AuthCentralDataProtectionStartup.GetConfiguration(_config));
            services.AddMvc();
            services.AddMembershipReboot(_config);
            services.AddAuthorizationPolicies();
            services.AddAuthCentralDependencies(_config);
            services.AddIdentityServer(_config.DB.IdentityServer3);
            services.TryAddTransient<IBuildPaginationLinks, PaginationLinkBuilder>();
            services.ConfigureRouting( routeOptions => {
                // All generated URL's should append a trailing slash.
                routeOptions.AppendTrailingSlash = true;

                // All generated URL's should be lower-case.
                routeOptions.LowercaseUrls = true;
            });
            services.ConfigureAntiforgery(c =>
            {
                c.CookieName = "fsw.authcentral.xsrf";
            });
        }

        public void Configure(IApplicationBuilder app, IApplicationEnvironment env, ILoggerFactory loggerFactory, StoreSettings idSvrStoreSettings)
        {
            app.ConfigureLoggers(loggerFactory, _config.IsDebug);
            loggerFactory.AddSerilog();
            loggerFactory.AddProvider(new LogCentral.MicrosoftFramework.LoggingProvider(_config.Log4NetConfigPath));
            app.UseMiddleware<LogCentral.MicrosoftFramework.LogMiddleware>();

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
            AdminUserAccountServiceContainer container = app.ApplicationServices.GetService<AdminUserAccountServiceContainer>();
            UserAccountService<HierarchicalUserAccount> idmRepo = container.Service;
            IClientService idsRepo = app.ApplicationServices.GetService<IClientService>();
            HealthChecker.ScheduleHealthCheck(_config, loggerFactory, idsRepo, idmRepo);

            app.Map(_config.Uri.AuthorityMapPath, ids =>
            {
                ids.UseIdentityServer(env, loggerFactory, _config, idSvrStoreSettings);
            });

            app.UseMvc();
        }
    }
}
