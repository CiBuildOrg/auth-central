using System;

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    class MembershipRebootSetup: MembershipRebootConfiguration<HierarchicalUserAccount>
    {
        private static MembershipRebootSetup TheOneInstance;
        private static object InstanceLock = new object();

        private MembershipRebootSetup(): base() { }
        private MembershipRebootSetup(SecuritySettings securitySettings) : base(securitySettings) { }
        
        public static MembershipRebootSetup GetConfig(IApplicationBuilder app)
        {
            if(TheOneInstance == null)
            {
                lock (InstanceLock)
                {
                    if (TheOneInstance == null)
                    {
                        TheOneInstance = CreateNewInstance(app);
                    }
                }
            }

            return TheOneInstance;
        }
 
        private static MembershipRebootSetup CreateNewInstance(IApplicationBuilder app)
        {
            SecuritySettings securitySettings = new SecuritySettings();

            var newInstance = new MembershipRebootSetup(securitySettings)
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
               VerificationKeyLifetime = new TimeSpan(0,20, 0) // password reset, change email, & change mobile verfication key lifetime
            };

            newInstance.ConfigurePasswordComplexity(minimumLength: 7, minimumNumberOfComplexityRules: 3);

            var appinfo = new AuthCentralAppInfo(
                app,
                "FSW", 
                "Copyright fsw.com 2015",
                "UserAccount", 
                "UserAccount/ChangeEmail/Confirm/",
                "UserAccount/Register/Cancel/",
                "UserAccount/PasswordReset/Confirm/");

            var emailFormatter = new EmailMessageFormatter<HierarchicalUserAccount>(appinfo);
            newInstance.AddEventHandler(new DebuggerEventHandler<HierarchicalUserAccount>());
            newInstance.AddEventHandler(new EmailAccountEventsHandler<HierarchicalUserAccount>(emailFormatter));
            //newInstance.AddEventHandler(new TwilioSmsEventHandler(appinfo));

            return newInstance;
        }

        private class AuthCentralAppInfo : RelativePathApplicationInformation
        {
            IApplicationBuilder app;

            public AuthCentralAppInfo(
                IApplicationBuilder app,
                string appName,
                string emailSig,
                string relativeLoginUrl,
                string relativeConfirmChangeEmailUrl,
                string relativeCancelNewAccountUrl,
                string relativeConfirmPasswordResetUrl)
            {
                this.app = app;
                app.Use((ctx, next) =>
                {
                    this.SetBaseUrl(GetApplicationBaseUrl(ctx));
                    return next();
                });
                this.ApplicationName = appName;
                this.EmailSignature = emailSig;
                this.RelativeLoginUrl = relativeLoginUrl;
                this.RelativeCancelVerificationUrl = relativeCancelNewAccountUrl;
                this.RelativeConfirmPasswordResetUrl = relativeConfirmPasswordResetUrl;
                this.RelativeConfirmChangeEmailUrl = relativeConfirmChangeEmailUrl;
            }

            string GetApplicationBaseUrl(HttpContext ctx)
            {
                var tmp = ctx.Request.Scheme + "://" + ctx.Request.Host;
                if (ctx.Request.PathBase.HasValue)
                {
                    if (!ctx.Request.PathBase.Value.StartsWith("/"))
                    {
                        tmp += "/";
                    }
                    tmp += ctx.Request.PathBase.Value;
                }
                else
                {
                    tmp += "/";
                }
                if (!tmp.EndsWith("/")) tmp += "/"; 
                return tmp;
            }
        }
    }
}
