﻿using System;

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;

using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Microsoft.Extensions.PlatformAbstractions;
using Fsw.Enterprise.AuthCentral.IdMgr.Notifications.Email;
using Fsw.Enterprise.AuthCentral.IdMgr.Events;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    internal class MembershipRebootSetup: MembershipRebootConfiguration<HierarchicalUserAccount>
    {
        private static MembershipRebootSetup TheOneInstance;
        private static object InstanceLock = new object();

        private MembershipRebootSetup(SecuritySettings securitySettings) : base(securitySettings) { }
        
        public static MembershipRebootSetup GetConfig(IHttpContextAccessor contextAccessor, IApplicationEnvironment appEnv, EnvConfig config)
        {
            if(TheOneInstance == null)
            {
                lock (InstanceLock)
                {
                    if (TheOneInstance == null)
                    {
                        TheOneInstance = CreateNewInstance(contextAccessor, appEnv, config);
                    }
                }
            }

            return TheOneInstance;
        }
 
        private static MembershipRebootSetup CreateNewInstance(IHttpContextAccessor contextAccessor, IApplicationEnvironment appEnv, EnvConfig config)
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
                contextAccessor,
                "FSW Auth Central", 
                "Copyright fsw.com 2015",
                "useraccount/profile", 
                "useraccount/changeemail/confirm/",
                "useraccount/register/cancel/",
                "useraccount/passwordreset/confirm/");

            var emailBodyType = AuthCentralSmtpMessageDelivery.MsgBodyTypes.MultipartAlternativeAsJson;
            var emailFormatter = new AuthCentralEmailMessageFormatter(appEnv, appinfo, emailBodyType);
            var smtpMsgDelivery = new AuthCentralSmtpMessageDelivery(config, emailBodyType);

            newInstance.AddEventHandler(new DebuggerEventHandler<HierarchicalUserAccount>());
            newInstance.AddEventHandler(new AuthCentralEmailEventsHandler<HierarchicalUserAccount>(emailFormatter, smtpMsgDelivery));
            //newInstance.AddEventHandler(new TwilioSmsEventHandler(appinfo));

            return newInstance;
        }
    }

    internal sealed class AuthCentralAppInfo : RelativePathApplicationInformation
    {
        public AuthCentralAppInfo(
            IHttpContextAccessor ctxAccessor,
            string appName,
            string emailSig,
            string relativeLoginUrl,
            string relativeConfirmChangeEmailUrl,
            string relativeCancelNewAccountUrl,
            string relativeConfirmPasswordResetUrl)
        {
            HttpContext ctx = ctxAccessor.HttpContext;
            SetBaseUrl(GetApplicationBaseUrl(ctx));
            ApplicationName = appName;
            EmailSignature = emailSig;
            RelativeLoginUrl = relativeLoginUrl;
            RelativeCancelVerificationUrl = relativeCancelNewAccountUrl;
            RelativeConfirmPasswordResetUrl = relativeConfirmPasswordResetUrl;
            RelativeConfirmChangeEmailUrl = relativeConfirmChangeEmailUrl;
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
