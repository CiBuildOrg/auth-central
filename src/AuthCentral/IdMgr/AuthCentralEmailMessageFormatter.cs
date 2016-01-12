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
        const string HTML_FILE_EXTENSION        = "html";
        const string PLAIN_TEXT_FILE_EXTENSION  = "txt";
        const string TEMPLATE_PATH_NO_EXTENSION = "IdMgr/EmailTemplates/{0}";

        private readonly IApplicationEnvironment _appEnvironment;

        public AuthCentralEmailMessageFormatter(IApplicationEnvironment appEnvironment, AuthCentralAppInfo appInfo) : base(appInfo)
        {
            _appEnvironment = appEnvironment;
        }

        public AuthCentralEmailMessageFormatter(IApplicationEnvironment appEnvironment, Lazy<ApplicationInformation> appInfo) : base(appInfo)
        {
            _appEnvironment = appEnvironment;
        }

        protected override string GetSubject(UserAccountEvent<HierarchicalUserAccount> evt, IDictionary<string, string> values)
        {
            return FormatValue(evt, LoadSubjectTemplate(evt), values);
        }

        protected override string GetBody(UserAccountEvent<HierarchicalUserAccount> evt, IDictionary<string, string> values)
        {
            return FormatValue(evt, LoadBodyTemplate(evt), values);
        }

        
        protected override string LoadSubjectTemplate(UserAccountEvent<HierarchicalUserAccount> evt)
        {
            return LoadTemplate(CleanGenericName(evt.GetType()) + "_Subject." + PLAIN_TEXT_FILE_EXTENSION);
        }

        protected override string LoadBodyTemplate(UserAccountEvent<HierarchicalUserAccount> evt)
        {
            return LoadTemplate(CleanGenericName(evt.GetType()) + "_Body." + HTML_FILE_EXTENSION);
        }

        string LoadTemplate(string name)
        {
            name = String.Format(TEMPLATE_PATH_NO_EXTENSION, name);

            var file = Path.Combine(_appEnvironment.ApplicationBasePath, name);
            using (var s = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                if (s == null) return null;
                using (var sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private string CleanGenericName(Type type)
        {
            var name = type.Name;
            var idx = name.IndexOf('`');
            if (idx > 0)
            {
                name = name.Substring(0, idx);
            }
            return name;
        }

    }
}
