using System;
using System.Collections.Generic;
using System.IO;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using System.Text;

namespace Fsw.Enterprise.AuthCentral.IdMgr.Notifications.Email
{
    internal class AuthCentralEmailMessageFormatter : EmailMessageFormatter<HierarchicalUserAccount>
    {
        const string HTML_FILE_EXTENSION        = "html";
        const string PLAIN_TEXT_FILE_EXTENSION  = "txt";
        const string TEMPLATE_PATH_NO_EXTENSION = "IdMgr/Notifications/Email/Templates/{0}";

        private readonly IApplicationEnvironment _appEnvironment;
        private readonly AuthCentralSmtpMessageDelivery.MsgBodyTypes _msgBodyType;

        public AuthCentralEmailMessageFormatter(IApplicationEnvironment appEnvironment, AuthCentralAppInfo appInfo, 
                                                AuthCentralSmtpMessageDelivery.MsgBodyTypes msgBodyType) : base(appInfo) {
            _appEnvironment = appEnvironment;
            _msgBodyType = msgBodyType;
        }

        public AuthCentralEmailMessageFormatter(IApplicationEnvironment appEnvironment, Lazy<ApplicationInformation> appInfo, 
                                                AuthCentralSmtpMessageDelivery.MsgBodyTypes msgBodyType) : base(appInfo) {
            _appEnvironment = appEnvironment;
            _msgBodyType = msgBodyType;
        }

        protected override string GetSubject(UserAccountEvent<HierarchicalUserAccount> evt, IDictionary<string, string> values) {
            return FormatValue(evt, LoadSubjectTemplate(evt), values);
        }

        protected override string GetBody(UserAccountEvent<HierarchicalUserAccount> evt, IDictionary<string, string> values) {
            string msgBody = string.Empty;
            
            switch (_msgBodyType)
            {
                case AuthCentralSmtpMessageDelivery.MsgBodyTypes.MultipartAlternativeAsJson:
                {
                    var multipartMsgBody = new MultipartMessageBody()
                    {
                        PlainText = FormatValue(evt, LoadBodyTemplate(evt, PLAIN_TEXT_FILE_EXTENSION), values),
                        Html = FormatValue(evt, LoadBodyTemplate(evt, HTML_FILE_EXTENSION), values),
                    };

                    msgBody = JsonConvert.SerializeObject(multipartMsgBody);
                    break;
                }
                case AuthCentralSmtpMessageDelivery.MsgBodyTypes.Html:
                {
                    msgBody = FormatValue(evt, LoadBodyTemplate(evt, HTML_FILE_EXTENSION), values);
                    break;
                }
                default:
                {
                    msgBody = FormatValue(evt, LoadBodyTemplate(evt, PLAIN_TEXT_FILE_EXTENSION), values);
                    break;
                }
            }

            return msgBody;
        }
        
        protected override string LoadSubjectTemplate(UserAccountEvent<HierarchicalUserAccount> evt) {
            return LoadTemplate(CleanGenericName(evt.GetType()) + "_Subject." + PLAIN_TEXT_FILE_EXTENSION);
        }

        private string LoadBodyTemplate(UserAccountEvent<HierarchicalUserAccount> evt, string extension) {
            StringBuilder bodyTemplate = new StringBuilder(LoadTemplate(CleanGenericName(evt.GetType()) + "_Body." + extension));

            if(extension == HTML_FILE_EXTENSION)
            {
                bodyTemplate.Insert(0, LoadTemplate("CommonHeader_Body.html"));
                bodyTemplate.Append(LoadTemplate("CommonFooter_Body.html"));
            }

            return bodyTemplate.ToString();
        }

        protected override string LoadBodyTemplate(UserAccountEvent<HierarchicalUserAccount> evt) {
            throw new NotImplementedException();
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
