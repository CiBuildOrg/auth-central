using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.Crypto;
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
        EventBusUserAccountRepository<TAccount> repo;

        public AdminUserAccountService(MembershipRebootConfiguration<TAccount> config, IUserAccountRepository<TAccount> repo) : base(config, repo)
        {
            this.repo = new EventBusUserAccountRepository<TAccount>(this, repo, config.ValidationBus, config.EventBus);
        }

        public TAccount CreateAccount(string username, string email, Guid? id = null, DateTime? dateCreated = null, IEnumerable<Claim> claims = null)
        {
            return CreateAccount(null, username, email, id, dateCreated, null, claims);
        }

        public TAccount CreateAccount(string tenant, string username, string email, Guid? id = null, DateTime? dateCreated = null, TAccount account = null, IEnumerable<Claim> claims = null)
        {
            if (Configuration.EmailIsUsername)
            {
                Tracing.Verbose("[UserAccountService.CreateAccount] applying email is username");
                username = email;
            }

            if (!Configuration.MultiTenant)
            {
                Tracing.Verbose("[UserAccountService.CreateAccount] applying default tenant");
                tenant = Configuration.DefaultTenant;
            }

            Tracing.Information("[UserAccountService.CreateAccount] called: {0}, {1}, {2}", tenant, username, email);

            account = account ?? CreateUserAccount();
            Init(account, tenant, username, PasswordGenerator.GeneratePasswordOfLength(16), email, id, dateCreated, claims);

            ValidateEmail(account, email);
            ValidateUsername(account, username);
            // ValidatePassword(account, password);
            SetConfirmedEmail(account.ID, account.Email);
            ResetPassword(account.ID);

            Tracing.Verbose("[UserAccountService.CreateAccount] success");

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

            return account;
        }
    }
}
