using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr;
using Fsw.Enterprise.AuthCentral.Testing;
using Fsw.Enterprise.AuthCentral.MongoStore;
using Fsw.Enterprise.AuthCentral.MongoStore.Admin;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.OpenIdConnect;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using MongoDB.Driver;
using Serilog;

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
            services.AddScoped<IClientService, MemoryClientService>();
            services.AddScoped<IScopeService, MemoryScopeService>();
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

        public static void AddMembershipReboot(this IServiceCollection services, string mrConnectionString)
        {
            services.AddScoped(provider => MembershipRebootSetup.GetConfig(provider.GetService<IApplicationBuilder>(), provider.GetService<IApplicationEnvironment>()));
            services.AddAuthentication(sharedOptions => sharedOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
            services.AddScoped<MembershipRebootConfiguration<HierarchicalUserAccount>>(provider => MembershipRebootSetup.GetConfig(null, provider.GetService<IApplicationEnvironment>()));
			DIContainerRegistrations.ConfigureIServiceCollection(services);
        }

        public static void AddAuthCentralDependencies(this IServiceCollection services, EnvConfig config)
        {
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
