using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    public class AdminUserAccountService<TAccount> : UserAccountService<TAccount> where TAccount : UserAccount
    {

        public AdminUserAccountService(IUserAccountRepository<TAccount> repo) : base(repo) { }
        public AdminUserAccountService(MembershipRebootConfiguration<TAccount> config, IUserAccountRepository<TAccount> repo) : base(config, repo) { }
            
        public override TAccount CreateAccount(string tenant, string username, string password, string email, Guid? id = null, DateTime? dateCreated = null, TAccount account = null, IEnumerable<Claim> claims = null)
        {
            TAccount newAccount = base.CreateAccount(tenant, username, password, email, id, dateCreated, account, claims);

            IEventSource source = this;
            IEnumerable<IEvent> events = source.GetEvents();

            // Eventually, we could generate a verification key ourselves.
            // In the meantime, this will commandeer the key from the password reset event.
            PasswordResetRequestedEvent<TAccount> passwordResetEvent = 
                events.Single(e => e is PasswordResetRequestedEvent<TAccount>) as PasswordResetRequestedEvent<TAccount>;

            source.Clear();

            UserAccountCreatedByAdminEvent<TAccount> accountCreatedEvent = 
                new UserAccountCreatedByAdminEvent<TAccount> { VerificationKey = passwordResetEvent.VerificationKey };
            AddEvent(accountCreatedEvent);

            return newAccount;
        }
    }
}
