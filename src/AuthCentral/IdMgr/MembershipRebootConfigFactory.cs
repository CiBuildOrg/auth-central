﻿using System;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr.Notifications.Email;
using Microsoft.Extensions.PlatformAbstractions;
using Fsw.Enterprise.AuthCentral.IdMgr.Notifications.Email.EventHandlers;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    /// <summary>
    /// Factory for creating new Admin-tool <see cref="MembershipRebootConfiguration{HierarchicalUserAccount}"/> objects.
    /// </summary>
    public static class MembershipRebootConfigFactory
    {
        private static MembershipRebootSetup DefaultInstance;
        private static MembershipRebootSetup AdminInstance;

        private static object InstanceLock = new object();
        internal static MembershipRebootSetup GetDefaultConfig(IApplicationEnvironment appEnv, EnvConfig config)
        {
            if (DefaultInstance == null)
            {
                lock (InstanceLock)
                {
                    if (DefaultInstance == null)
                    {
                        DefaultInstance = CreateDefaultInstance(appEnv, config);
                    }
                }
            }

            return DefaultInstance;
        }

        internal static MembershipRebootSetup GetAdminConfig(IApplicationEnvironment appEnv, EnvConfig config)
        {
            if (AdminInstance == null)
            {
                lock (InstanceLock)
                {
                    if (AdminInstance == null)
                    {
                        AdminInstance = CreateAdminInstance(appEnv, config);
                    }
                }
            }

            return AdminInstance;
        }

        private static MembershipRebootSetup CreateDefaultInstance(IApplicationEnvironment appEnv, EnvConfig config)
        {
            SecuritySettings securitySettings = new SecuritySettings();
            MembershipRebootSetup newInstance = GetDefaultConfigInstance(config.Uri.IssuerUri, securitySettings);
            AuthCentralAppInfo appInfo = BuildAppInfo(config.Uri.IssuerUri);

            var emailBodyType = AuthCentralSmtpMessageDelivery.MsgBodyTypes.MultipartAlternativeAsJson;
            var emailFormatter = new AuthCentralEmailMessageFormatter(appEnv, appInfo, emailBodyType);
            var smtpMsgDelivery = new AuthCentralSmtpMessageDelivery(config, emailBodyType);

            newInstance.AddEventHandler(new DefaultEmailEventHandler(emailFormatter, smtpMsgDelivery));
            newInstance.AddEventHandler(new UserEmailEventHandler(emailFormatter, smtpMsgDelivery));
            
            return newInstance;
        }

        private static MembershipRebootSetup CreateAdminInstance(IApplicationEnvironment appEnv, EnvConfig config)
        {
            SecuritySettings securitySettings = new SecuritySettings();
            MembershipRebootSetup newInstance = GetDefaultConfigInstance(config.Uri.IssuerUri, securitySettings);
            AuthCentralAppInfo appInfo = BuildAppInfo(config.Uri.IssuerUri);

            var emailBodyType = AuthCentralSmtpMessageDelivery.MsgBodyTypes.MultipartAlternativeAsJson;
            var emailFormatter = new AuthCentralEmailMessageFormatter(appEnv, appInfo, emailBodyType);

            emailFormatter.BodyTemplateNameResolverOverrides.Add(
                typeof(PasswordResetRequestedEvent<HierarchicalUserAccount>), 
                (evt, extension) => "AccountCreatedAdminEvent_Body." + extension
            );

            emailFormatter.SubjectTemplateNameResolverOverrides.Add(
                typeof(PasswordResetRequestedEvent<HierarchicalUserAccount>),
                evt => "AccountCreatedEvent_Subject.txt"
            );

            var smtpMsgDelivery = new AuthCentralSmtpMessageDelivery(config, emailBodyType);            
            newInstance.AddEventHandler(new DefaultEmailEventHandler(emailFormatter, smtpMsgDelivery));

            return newInstance;
        }

        private static MembershipRebootSetup GetDefaultConfigInstance(string baseUrl, SecuritySettings securitySettings)
        {
            var newInstance = new MembershipRebootSetup
            {
                SecuritySettings = securitySettings,
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

            newInstance.ConfigurePasswordComplexity(minimumLength: 7, minimumNumberOfComplexityRules: 3);
            
            newInstance.AddEventHandler(new DebuggerEventHandler<HierarchicalUserAccount>());

            return newInstance;
        }

        private static AuthCentralAppInfo BuildAppInfo(string baseUrl)
        {
            return new AuthCentralAppInfo(
                baseUrl,
                "FSW Auth Central",
                "Copyright fsw.com 2015",
                "useraccount/details",
                "useraccount/changeemail/confirm/",
                "useraccount/register/cancel/",
                "useraccount/passwordreset/confirm/");
        }

    }
}


