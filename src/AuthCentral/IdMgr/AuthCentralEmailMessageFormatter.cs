using System;
using System.Collections.Generic;
using System.IO;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Microsoft.Extensions.PlatformAbstractions;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    internal class AuthCentralEmailMessageFormatter : EmailMessageFormatter<HierarchicalUserAccount>
    {
        private readonly IApplicationEnvironment _appEnvironment;

        public AuthCentralEmailMessageFormatter(IApplicationEnvironment appEnvironment, AuthCentralAppInfo appInfo) : base(appInfo)
        {
            _appEnvironment = appEnvironment;
        }

        public AuthCentralEmailMessageFormatter(IApplicationEnvironment appEnvironment, Lazy<ApplicationInformation> appInfo) : base(appInfo)
        {
            _appEnvironment = appEnvironment;
        }

        protected override string GetBody(UserAccountEvent<HierarchicalUserAccount> evt, IDictionary<string, string> values)
        {
            if(evt.GetType() != typeof (PasswordResetRequestedEvent<HierarchicalUserAccount>))
                return base.GetBody(evt, values);

            string preBody;
            var file = Path.Combine(_appEnvironment.ApplicationBasePath, @"IdMgr/EmailTemplates/PasswordResetRequestedEvent_Body.html");
            using (var s = new FileStream(file, FileMode.Open))
            {
                using (var sr = new StreamReader(s))
                {
                    preBody = sr.ReadToEnd();
                }
            }

            return FormatValue(evt, preBody, values);
        }
    }
}
