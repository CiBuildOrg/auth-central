using BrockAllen.MembershipReboot;
using Fsw.Enterprise.AuthCentral.Crypto;
using Fsw.Enterprise.AuthCentral.IdMgr.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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

            IEventSource source = this;

            account = account ?? CreateUserAccount();

            Init(account, tenant, username, PasswordGenerator.GeneratePasswordOfLength(16), email, id, dateCreated, claims);
            ValidateEmail(account, email);
            ValidateUsername(account, username);

            source.Clear();
            repo.Add(account);

            SetConfirmedEmail(account.ID, email);

            // this is important, or we reset the IsAccountVerified flag to false in our update.
            account = GetByID(account.ID);

            var accountCreatedEvent = new UserAccountCreatedByAdminEvent<TAccount>
            {
                Account = account,
                VerificationKey = SetVerificationKey(account, VerificationKeyPurpose.ResetPassword)
            };

            AddEvent(accountCreatedEvent);
            repo.Update(account);

            Tracing.Verbose("[UserAccountService.CreateAccount] success");

            return account;
        }

        public override void Update(TAccount account)
        {
            IEventSource source = this;
            source.Clear();

            base.Update(account);
        }
    }
}
