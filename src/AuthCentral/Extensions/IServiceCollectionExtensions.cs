using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr;
using Fsw.Enterprise.AuthCentral.MongoDb;
using Fsw.Enterprise.AuthCentral.MongoStore;
using Fsw.Enterprise.AuthCentral.MongoStore.Admin;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.OpenIdConnect;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using MongoDB.Driver;
using Serilog;
using System.Security.Claims;
using MongoDatabase = Fsw.Enterprise.AuthCentral.MongoDb.MongoDatabase;

namespace Fsw.Enterprise.AuthCentral.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddIdentityServer(this IServiceCollection services, string idsConnectionString)
        {
            StoreSettings idSvrStoreSettings = StoreSettings.DefaultSettings();
            idSvrStoreSettings.ConnectionString = idsConnectionString;
            idSvrStoreSettings.Database = MongoUrl.Create(idSvrStoreSettings.ConnectionString).DatabaseName;
            services.AddInstance(idSvrStoreSettings);
            services.AddInstance(AdminServiceFactory.CreateClientService(idSvrStoreSettings));
            services.AddInstance(AdminServiceFactory.CreateScopeService(idSvrStoreSettings));
        }

        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // AuthorizeAttribute seems to accept multiple policies, so making these less cumulative might be useful.
                options.AddPolicy("FswPlatform", policy => {
                    policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
                    policy.RequireClaim("scope", "fsw_platform");
                });

                options.AddPolicy("FswAdmin", policy => {
                    policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
                    policy.RequireClaim("scope", "fsw_platform");
                    policy.RequireClaim("fsw:authcentral:admin", "true");
                });

                options.AddPolicy("FswAutomation", policy =>
                {
                    policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
                    policy.RequireClaim("scope", "fsw_platform");
                    policy.RequireClaim("fsw:authcentral:admin", "true");
                    policy.RequireClaim("fsw:testautomation", "true");
                });

                options.DefaultPolicy = options.GetPolicy("FswPlatform");
            });
        }

        public static void AddMembershipReboot(this IServiceCollection services, EnvConfig config)
        {
            // any middleware or component that uses DI to inject an instance of UserAccountService<HierarchicalUserAccount>
            // should instead depend on either AdminUserAccountServiceContainer, or DefaultUserAccountServiceContainer
            services.AddScoped(provider =>
            {
                MembershipRebootSetup setup = MembershipRebootConfigFactory.GetAdminConfig(provider.GetService<IApplicationEnvironment>(), provider.GetRequiredService<ILoggerFactory>(), config);
                var repository = provider.GetRequiredService<IUserAccountRepository<HierarchicalUserAccount>>();
                return new AdminUserAccountServiceContainer
                {
                    Service = new UserAccountService<HierarchicalUserAccount>(setup, repository)
                };
            });

            services.AddScoped(provider =>
            {
                MembershipRebootSetup setup = MembershipRebootConfigFactory.GetDefaultConfig(provider.GetService<IApplicationEnvironment>(), provider.GetRequiredService<ILoggerFactory>(), config);
                var repository = provider.GetRequiredService<IUserAccountRepository<HierarchicalUserAccount>>();
                return new DefaultUserAccountServiceContainer
                {
                    Service = new UserAccountService<HierarchicalUserAccount>(setup, repository)
                };
            });

            services.AddScoped(typeof(IUserAccountRepository<HierarchicalUserAccount>), typeof(MongoUserAccountRepository<HierarchicalUserAccount>));
            services.AddScoped<IBulkUserRepository<HierarchicalUserAccount>, MongoUserAccountRepository<HierarchicalUserAccount>>();
            
            services.AddAuthentication(sharedOptions => sharedOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
            services.AddScoped(provider => new MongoDatabase(config.DB.MembershipReboot));
        }

        public static void AddAuthCentralDependencies(this IServiceCollection services, EnvConfig config)
        {
            services.AddScoped<MongoAuthenticationService>();
            services.AddScoped(typeof(AuthenticationService<HierarchicalUserAccount>), typeof(MongoAuthenticationService));
            services.AddInstance<EnvConfig>(config);
        }

        public static void AddSerilog(this IServiceCollection services, bool isDebug)
        {
            var loggerConfig = new LoggerConfiguration()
               .WriteTo.Trace()
               .WriteTo.Console();

            if (isDebug)
            {
                loggerConfig.MinimumLevel.Verbose();
            }
            else
            {
                loggerConfig.MinimumLevel.Error();
            }

            Log.Logger = loggerConfig.CreateLogger();
        }
    }
}
