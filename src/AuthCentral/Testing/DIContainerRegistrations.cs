using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr;
using IdentityServer3.Core.Configuration;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fsw.Enterprise.AuthCentral.Testing
{
    public static class DIContainerRegistrations
    {
        public static void ConfigureTestUserService(this IdentityServerServiceFactory factory, IApplicationBuilder app)
        {
            factory.Register(new Registration<UserAccount, HierarchicalUserAccount>());
            factory.Register(new Registration<MembershipRebootConfiguration<HierarchicalUserAccount>>(x => MembershipRebootSetup.GetConfig(app)));
            factory.Register(new Registration<IUserAccountRepository<HierarchicalUserAccount>, TestUserRepository>());
            factory.Register(new Registration<UserAccountService<HierarchicalUserAccount>>());
        }

        public static void ConfigureIServiceCollection(IServiceCollection services)
        {
            services.AddScoped<MembershipRebootConfiguration<HierarchicalUserAccount>>(provider => MembershipRebootSetup.GetConfig(null));
            services.AddScoped<UserAccountService<HierarchicalUserAccount>>();
            services.AddScoped(typeof(IUserAccountRepository<HierarchicalUserAccount>), typeof(TestUserRepository));
            services.AddScoped<MongoAuthenticationService>();
        }
    }
}