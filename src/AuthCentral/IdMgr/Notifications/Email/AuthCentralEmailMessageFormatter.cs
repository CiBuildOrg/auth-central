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

        /// <summary>
        ///     Method definition required when overriding Subject Template Name resolution.
        /// </summary>
        /// <param name="evt">The event for which to return the Subject TEmplate Name.</param>
        /// <returns>The name of the subject template to load (expected to be in the same directory as all other templates).</returns>
        public delegate string ResolveSubjectTemplateName(UserAccountEvent<HierarchicalUserAccount> evt);

        /// <summary>
        ///     Method definition required when overriding Body Template Name resolution.
        /// </summary>
        /// <param name="evt">The event for which to return the Body TEmplate Name.</param>
        /// <returns>The name of the body template to load (expected to be in the same directory as all other templates).</returns>
        public delegate string ResolveBodyTemplateName(UserAccountEvent<HierarchicalUserAccount> evt, string fileExtension);

        /// <summary>
        ///     A collection of <typeparamref name="ResolveBodyTemplateName"/>s used to override the default 
        ///     body template name resolution behavior.
        /// </summary>
        public IDictionary<Type, ResolveBodyTemplateName> BodyTemplateNameResolverOverrides = new Dictionary<Type, ResolveBodyTemplateName>();

        /// <summary>
        ///     A collection of <typeparamref name="ResolveSubjectTemplateName"/>s used to override the default 
        ///     subject template name resolution behavior.
        /// </summary>
        public IDictionary<Type, ResolveSubjectTemplateName> SubjectTemplateNameResolverOverrides = new Dictionary<Type, ResolveSubjectTemplateName>();


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
            ResolveSubjectTemplateName resolveSubjectTemplateName;

            if(!SubjectTemplateNameResolverOverrides.TryGetValue(evt.GetType(), out resolveSubjectTemplateName)) {
                resolveSubjectTemplateName = DefaultSubjectTemplateNameResolver;
            }

            string templateName = resolveSubjectTemplateName(evt);

            return LoadTemplate(templateName);
        }

        public virtual string LoadBodyTemplate(UserAccountEvent<HierarchicalUserAccount> evt, string extension) {
            ResolveBodyTemplateName resolveBodyTemplateName;

            if(!BodyTemplateNameResolverOverrides.TryGetValue(evt.GetType(), out resolveBodyTemplateName)) {
                resolveBodyTemplateName = DefaultBodyTemplateNameResolver;
            }

            string templateName = resolveBodyTemplateName(evt, extension);
            StringBuilder bodyTemplate = new StringBuilder(LoadTemplate(templateName));

            if(extension == HTML_FILE_EXTENSION)
            {
                bodyTemplate.Insert(0, LoadTemplate("CommonHeader_Body.html"));
                bodyTemplate.Append(LoadTemplate("CommonFooter_Body.html"));
            }

            return bodyTemplate.ToString();
        }

        // intentionally hide derrived type member
        protected string LoadBodyTemplate(UserAccountEvent<HierarchicalUserAccount> evt) {
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

        private static string CleanGenericName(Type type)
        {
            var name = type.Name;
            var idx = name.IndexOf('`');
            if (idx > 0)
            {
                name = name.Substring(0, idx);
            }
            return name;
        }

        private static string DefaultSubjectTemplateNameResolver(UserAccountEvent<HierarchicalUserAccount> evt)
        {
            return CleanGenericName(evt.GetType()) + "_Subject." + PLAIN_TEXT_FILE_EXTENSION;
        }
        
        private static string DefaultBodyTemplateNameResolver(UserAccountEvent<HierarchicalUserAccount> evt, string fileExtension)
        {
            return CleanGenericName(evt.GetType()) + "_Body." + fileExtension;
        }

    }
}
