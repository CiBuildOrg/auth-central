using BrockAllen.MembershipReboot;
using Fsw.Enterprise.AuthCentral.Crypto;
using Fsw.Enterprise.AuthCentral.IdMgr.Events;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using BrockAllen.MembershipReboot.Hierarchical;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    /// <summary>
    /// A user account service for use by user admin tools.
    /// </summary>
    public class AdminUserAccountService : UserAccountService<HierarchicalUserAccount>
    {
        /// <summary>
        /// Creates a new instance of <see cref="AdminUserAccountService"/> with the given <paramref name="config"/> and user <paramref name="repo"/>
        /// </summary>
        /// <param name="config">The configuration containing events and validation.</param>
        /// <param name="repo">The user repository in which to read / write user data.</param>
        public AdminUserAccountService(MembershipRebootConfiguration<HierarchicalUserAccount> config, IUserAccountRepository<HierarchicalUserAccount> repo) : base(config, repo)
        {
        }

        /// <summary>
        /// Creates a new account for a user by an admin.
        /// </summary>
        /// <param name="username">Username for the new user.</param>
        /// <param name="email">New user's email address</param>
        /// <param name="id">Unique identifier for the user.  Can be null and generated internally.</param>
        /// <param name="dateCreated">Date the user was created. Can be null and set to the current date and time.</param>
        /// <param name="claims">Claims the user will have upon creation.  Can be null.</param>
        /// <returns>The account that has been created with udpated details.</returns>
        /// <exception cref="NullReferenceException">account not succesfully created.</exception>
        public HierarchicalUserAccount CreateAccount(string username, string email, Guid? id = null, DateTime? dateCreated = null, IEnumerable<Claim> claims = null)
        {
            return CreateAccount(null, username, email, id, dateCreated, null, claims);
        }

        /// <summary>
        /// Creates a new account for a user by an admin.
        /// </summary>
        /// <param name="tenant">The user's tenant / membership group.</param>
        /// <param name="username">Username for the new user.</param>
        /// <param name="email">New user's email address</param>
        /// <param name="id">Unique identifier for the user.  Can be null and generated internally.</param>
        /// <param name="dateCreated">Date the user was created. Can be null and set to the current date and time.</param>
        /// <param name="account">A <see cref="HierarchicalUserAccount"/> object with user data already pre-loaded or empty.  Can be null and a blank account will be created.</param>
        /// <param name="claims">Claims the user will have upon creation.  Can be null.</param>
        /// <returns>The account that has been created with udpated details.</returns>
        /// <exception cref="NullReferenceException">account not succesfully created.</exception>
        public HierarchicalUserAccount CreateAccount(string tenant, string username, string email, Guid? id = null, DateTime? dateCreated = null, HierarchicalUserAccount account = null, IEnumerable<Claim> claims = null)
        {
            account = base.CreateAccount(tenant, username, PasswordGenerator.GeneratePasswordOfLength(16), email, id, dateCreated,
                account, claims);

            if(account == null)
                throw new NullReferenceException("account not succesfully created.");

            SetConfirmedEmail(account.ID, email);

            // this is important, or we reset the IsAccountVerified flag to false in our update.
            account = GetByID(account.ID);

            var accountCreatedEvent = new UserAccountCreatedByAdminEvent<HierarchicalUserAccount>
            {
                Account = account,
                VerificationKey = SetVerificationKey(account, VerificationKeyPurpose.ResetPassword)
            };

            AddEvent(accountCreatedEvent);
            Update(account);

            return account;
        }
    }
}
