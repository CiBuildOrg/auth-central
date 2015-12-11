using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.MongoDb;
using IdentityServer3.Core.Configuration;

using Fsw.Enterprise.AuthCentral.IdMgr;
using Microsoft.AspNet.Builder;

namespace Fsw.Enterprise.AuthCentral.Extensions
{
    public static class UserServiceExtensions
    {
        public static void ConfigureCustomUserService(this IdentityServerServiceFactory factory, IApplicationBuilder app, string connString)
        {
            factory.Register(new Registration<UserAccount, HierarchicalUserAccount>());
            factory.Register(new Registration<MembershipRebootConfiguration<HierarchicalUserAccount>>(x => MembershipRebootSetup.GetConfig(app)));
            factory.Register(new Registration<IUserAccountRepository<HierarchicalUserAccount>, MongoUserAccountRepository<HierarchicalUserAccount>>());
            factory.Register(new Registration<UserAccountService<HierarchicalUserAccount>>());
            factory.Register(new Registration<MongoDatabase>(resolver => new MongoDatabase(connString)));
        }
    }
}
