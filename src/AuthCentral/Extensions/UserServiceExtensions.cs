using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.MongoDb;
using IdentityServer3.Core.Configuration;

using Fsw.Enterprise.AuthCentral.IdMgr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;

namespace Fsw.Enterprise.AuthCentral.Extensions
{
    public static class UserServiceExtensions
    {
        public static void ConfigureCustomUserService(this IdentityServerServiceFactory factory, string connString, IApplicationEnvironment appEnv, ILoggerFactory loggerFactory, EnvConfig config)
        {
            factory.Register(new Registration<UserAccount, HierarchicalUserAccount>());
            factory.Register(new Registration<MembershipRebootConfiguration<HierarchicalUserAccount>>(x => MembershipRebootConfigFactory.GetDefaultConfig(appEnv, loggerFactory, config)));
            factory.Register(new Registration<IUserAccountRepository<HierarchicalUserAccount>, MongoUserAccountRepository<HierarchicalUserAccount>>());
            factory.Register(new Registration<UserAccountService<HierarchicalUserAccount>>());
            factory.Register(new Registration<MongoDatabase>(resolver => new MongoDatabase(connString)));
        }
    }
}
