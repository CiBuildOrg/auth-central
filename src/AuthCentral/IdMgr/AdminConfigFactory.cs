using System;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr.Notifications.Email;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.PlatformAbstractions;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    /// <summary>
    /// Factory for creating new Admin-tool <see cref="MembershipRebootConfiguration{HierarchicalUserAccount}"/> objects.
    /// </summary>
    public class AdminConfigFactory
    {
        /// <summary>
        /// Creates a new instance of a <see cref="MembershipRebootConfiguration{HierarchicalUserAccount}"/> from all AuthCentral configuration sources.
        /// </summary>
        /// <param name="app">Self-hosted web server application builder.  Contains application root paths.</param>
        /// <param name="appEnv">Application environment. used for runtime paths.</param>
        /// <param name="config">AuthCentral environment variables.  Contains configurations loaded from config files and the running environment.</param>
        /// <returns>A new, filled <see cref="MembershipRebootConfiguration{HierarchicalUserAccount}"/> object.</returns>
        public static MembershipRebootConfiguration<HierarchicalUserAccount> Create(IApplicationBuilder app, IApplicationEnvironment appEnv, EnvConfig config)
        {
            SecuritySettings securitySettings = new SecuritySettings();

            var newInstance = new MembershipRebootConfiguration<HierarchicalUserAccount>(securitySettings)
            {
                MultiTenant = false, // allow more than one tenant
                DefaultTenant = "fsw", // default tenant name used when no tenant is specified (used when MultiTenant is false)
                UsernamesUniqueAcrossTenants = true, // username must be unique across all tenants (normally username is only unique within each tenant)
                AllowAccountDeletion = true, // when account is closed, physically delete record from the database
                EmailIsUsername = false, // email is used as the user's username
                RequireAccountVerification = true, // email must be verified before use can log in
                AllowLoginAfterAccountCreation = true, // user can login immediately after account has been verified. 
                PasswordHashingIterationCount = 50000, // number of iterations used to hash passwords
                PasswordResetFrequency = 0, // number of days before passwords should be reset - 0 means never
                AccountLockoutFailedLoginAttempts = 5,  // lock user out after 5 failed attempts
                AccountLockoutDuration = new TimeSpan(0, 10, 0), // lock user out for 10 minutes
                VerificationKeyLifetime = new TimeSpan(0, 20, 0) // password reset, change email, & change mobile verfication key lifetime
            };

            newInstance.ConfigurePasswordComplexity(7, 3);

            var appinfo = new AuthCentralAppInfo(
                app,
                "FSW Auth Central",
                "Copyright fsw.com 2015",
                "UserAccount/Details",
                "UserAccount/ChangeEmail/Confirm/",
                "UserAccount/Register/Cancel/",
                "UserAccount/PasswordReset/Confirm/");

            var emailBodyType = AuthCentralSmtpMessageDelivery.MsgBodyTypes.MultipartAlternativeAsJson;
            var emailFormatter = new AuthCentralEmailMessageFormatter(appEnv, appinfo, emailBodyType);
            var smtpMsgDelivery = new AuthCentralSmtpMessageDelivery(config, emailBodyType);

            newInstance.AddEventHandler(new DebuggerEventHandler<HierarchicalUserAccount>());
            newInstance.AddEventHandler(new AdminEmailEventsHandler(emailFormatter, smtpMsgDelivery));

            return newInstance;
        }
    }
}