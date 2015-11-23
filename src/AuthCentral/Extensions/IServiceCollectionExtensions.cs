using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr;
using Fsw.Enterprise.AuthCentral.IdSvr;
using Fsw.Enterprise.AuthCentral.MongoDb;
using Fsw.Enterprise.AuthCentral.MongoStore;
using Fsw.Enterprise.AuthCentral.MongoStore.Admin;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.MembershipReboot;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.OpenIdConnect;
using Microsoft.Framework.DependencyInjection;
using MongoDB.Driver;
using Serilog;
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
            services.AddInstance<StoreSettings>(idSvrStoreSettings);
            services.AddInstance<IClientService>(AdminServiceFactory.CreateClientService(idSvrStoreSettings));
            services.AddInstance<IScopeService>(AdminServiceFactory.CreateScopeService(idSvrStoreSettings));
            services.AddScoped<ServiceFactory>(svc => new ServiceFactory(new Registration<IUserService>()));
            var usrSrv = new Registration<IUserService, MembershipRebootUserService<HierarchicalUserAccount>>();
            var idSvcfactory = new ServiceFactory(usrSrv, idSvrStoreSettings)
            {
                ViewService = new Registration<IViewService>(typeof(CustomViewService))
            };
        }

        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
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
        }

        public static void AddMembershipReboot(this IServiceCollection services, string mrConnectionString)
        {
            services.AddScoped(provider => MembershipRebootSetup.GetConfig(provider.GetService<IApplicationBuilder>()));
            services.AddAuthentication(sharedOptions => sharedOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
            services.AddScoped<UserAccountService<HierarchicalUserAccount>>();
            services.AddScoped(typeof(IUserAccountRepository<HierarchicalUserAccount>), typeof(MongoUserAccountRepository<HierarchicalUserAccount>));
            services.AddScoped(provider => new MongoDatabase(mrConnectionString));
        }

        public static void AddAuthCentralDependencies(this IServiceCollection services, EnvConfig config)
        {
            services.AddScoped<MongoAuthenticationService>();
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
