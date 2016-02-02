using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr;
using IdentityServer3.Core.Configuration;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Fsw.Enterprise.AuthCentral.Testing
{
    internal static class DiContainerRegistrations
    {
        public static void ConfigureTestUserService(this IdentityServerServiceFactory factory, IApplicationBuilder app, IApplicationEnvironment appEnvironment, EnvConfig config)
        {
            factory.Register(new Registration<UserAccount, HierarchicalUserAccount>());
            factory.Register(new Registration<MembershipRebootConfiguration<HierarchicalUserAccount>>(x => MembershipRebootConfigFactory.GetDefaultConfig(appEnvironment, config)));
            factory.Register(new Registration<IUserAccountRepository<HierarchicalUserAccount>, TestUserRepository>());
            factory.Register(new Registration<IBulkUserRepository<HierarchicalUserAccount>, TestBulkUserRepository>());
            factory.Register(new Registration<UserAccountService<HierarchicalUserAccount>>());
        }

        public static void ConfigureIServiceCollection(IServiceCollection services, IApplicationEnvironment env, EnvConfig config)
        {
            services.AddScoped<MembershipRebootConfiguration<HierarchicalUserAccount>>(provider => MembershipRebootConfigFactory.GetDefaultConfig(env, config));
            services.AddScoped<UserAccountService<HierarchicalUserAccount>>();
            services.AddScoped(typeof(IUserAccountRepository<HierarchicalUserAccount>), typeof(TestUserRepository));
            services.AddScoped<IBulkUserRepository<HierarchicalUserAccount>, TestBulkUserRepository>();
            services.AddScoped<MongoAuthenticationService>();
        }
    }
}