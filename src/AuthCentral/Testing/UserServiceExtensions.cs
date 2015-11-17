using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr;
using IdentityServer3.Core.Configuration;
using Microsoft.AspNet.Builder;

namespace Fsw.Enterprise.AuthCentral.Testing
{
    public static class UserServiceExtensions
    {
        public static void ConfigureTestUserService(this IdentityServerServiceFactory factory, IApplicationBuilder app, string connString)
        {
            factory.Register(new Registration<UserAccount, HierarchicalUserAccount>());
            factory.Register(new Registration<MembershipRebootConfiguration<HierarchicalUserAccount>>(x => MembershipRebootSetup.GetConfig(app)));
            factory.Register(new Registration<IUserAccountRepository<HierarchicalUserAccount>, TestUserRepository>());
            factory.Register(new Registration<UserAccountService<HierarchicalUserAccount>>());
        }
    }
}