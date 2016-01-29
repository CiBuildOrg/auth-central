using Microsoft.Extensions.Configuration;
using System;

namespace Fsw.Enterprise.AuthCentral
{
    public class EnvConfig
    {
        private UriConfig _uri;
        private CspConfig _csp;
        private CertConfig _cert;
        private SmtpConfig _smtp;
        private DatabaseConfig _db;
        private ClientConfig _client;
        private IConfigurationRoot _root;

        private static class EnvVars {
            public static string DbMembershipReboot = "AUTHCENTRAL_DB_MEMBERSHIPREBOOT";
            public static string DbIdentityServer3 = "AUTHCENTRAL_DB_IDENTITYSERVER3";
            public static string UriScheme = "AUTHCENTRAL_URI_SCHEME";
            public static string UriHost = "AUTHCENTRAL_URI_HOST";
            public static string UriPort = "AUTHCENTRAL_URI_PORT";
            public static string SmtpHost = "AUTHCENTRAL_SMTP_HOST";
            public static string SmtpFrom = "AUTHCENTRAL_SMTP_FROM";
            public static string SmtpPort = "AUTHCENTRAL_SMTP_PORT";
            public static string SmtpEnableSsl = "AUTHCENTRAL_SMTP_ENABLESSL";
            public static string SmtpUsername = "AUTHCENTRAL_SMTP_USERNAME";
            public static string SmtpPassword = "AUTHCENTRAL_SMTP_PASSWORD";
            public static string UriServiceRoot = "AUTHCENTRAL_URI_SERVICEROOT";
            public static string CertStoreName = "AUTHCENTRAL_CERT_STORENAME";
            public static string CertThumbprint = "AUTHCENTRAL_CERT_THUMBPRINT";
            public static string DebugMode = "AUTHCENTRAL_DEBUG_MODE";
            public static string ClientId = "AUTHCENTRAL_CLIENT_ID";
            public static string ClientSecret = "AUTHCENTRAL_CLIENT_SECRET";
            public static string Log4NetConfig = "AUTHCENTRAL_LOG4NET_CONFIG_PATH";
            public static string CspScriptSource = "AUTHCENTRAL_CSP_SCRIPT_SRC";
            public static string CspStyleSource = "AUTHCENTRAL_CSP_STYLE_SRC";
            public static string CspFontSource = "AUTHCENTRAL_CSP_FONT_SRC";
        }

        public EnvConfig(IConfigurationRoot root) {
            this._root = root;
            this._csp = new CspConfig(root);
            this._cert = new CertConfig(root);
            this._uri = new UriConfig(root);
            this._db = new DatabaseConfig(root);
            this._client = new ClientConfig(root);
            this._smtp = new SmtpConfig(root);
        }

        public CspConfig Csp {
            get { return this._csp; }
        }

        public CertConfig Cert {
            get { return this._cert; }
        }

        public SmtpConfig Smtp {
            get { return this._smtp; }
        }

        public ClientConfig Client {
            get { return this._client; }
        }

        public DatabaseConfig DB {
            get { return this._db; }
        }

        public UriConfig Uri
        {
            get { return this._uri; }
        }

        public bool IsDebug
        {
            get { return _root.Get<bool>(EnvVars.DebugMode); }
        }

        public string Log4NetConfigPath
        {
            get { return _root.Get<string>(EnvVars.Log4NetConfig); }
        }


        public class DatabaseConfig
        {
            private IConfigurationRoot _root;
            public DatabaseConfig(IConfigurationRoot root) {
                _root = root;
            }

            public string MembershipReboot
            {
                get
                {
                    return _root.Get<string>(EnvVars.DbMembershipReboot);
                }
            }

            public string IdentityServer3 
            { 
                get 
                {
                    return _root.Get<string>(EnvVars.DbIdentityServer3);
                } 
            }

         }

        public class UriConfig
        {
            private IConfigurationRoot _root;
            public UriConfig(IConfigurationRoot root)
            {
                _root = root;
            }

            public string AuthorityMapPath
            {
                get
                {
                    return "/auth";
                }
            } 

            public string LoginPath
            {
                get
                {
                    return this.AuthorityMapPath + "/login";
                }
            }

            public string Scheme
            {
                get
                {
                    return _root.Get<string>(EnvVars.UriScheme);
                }
            }

            public string Host 
            { 
                get 
                {
                    return _root.Get<string>(EnvVars.UriHost);
                } 
            }

            public int Port 
            { 
                get 
                {
                    return _root.Get<int>(EnvVars.UriPort);
                } 
            }

            public string IssuerUri
            {
                get
                {
                    return new UriBuilder(this.Scheme, this.Host, this.Port).Uri.AbsoluteUri;
                }
            }

            public string Authority
            {
                get
                {
                    return new UriBuilder(this.Scheme, this.Host, this.Port, this.AuthorityMapPath).Uri.AbsoluteUri;
                }
            }
        }


        public class CertConfig
        {
            private IConfigurationRoot _root;
            public CertConfig(IConfigurationRoot root)
            {
                _root = root;
            }

            public string StoreName
            {
                get
                {
                    return _root.Get<string>(EnvVars.CertStoreName);
                }
            }

            public string Thumbprint 
            { 
                get 
                {
                    return _root.Get<string>(EnvVars.CertThumbprint);
                } 
            }
        }

        public class SmtpConfig
        {
            private IConfigurationRoot _root;
            public SmtpConfig(IConfigurationRoot root)
            {
                _root = root;
            }

            public string Host
            {
                get
                {
                    return _root.Get<string>(EnvVars.SmtpHost);
                }
            }

            public string From 
            { 
                get 
                {
                    return _root.Get<string>(EnvVars.SmtpFrom);
                } 
            }

            public int Port => _root.Get<int>(EnvVars.SmtpPort);

            public bool EnableSsl => _root.Get<bool>(EnvVars.SmtpEnableSsl);

            public string Username => _root.Get<string>(EnvVars.SmtpUsername);

            public string Password => _root.Get<string>(EnvVars.SmtpPassword);
        }


        public class ClientConfig
        {
            private IConfigurationRoot _root;
            public ClientConfig(IConfigurationRoot root)
            {
                _root = root;
            }

            public string Id
            {
                get
                {
                    return _root.Get<string>(EnvVars.ClientId);
                }
            }

            public string Secret 
            { 
                get 
                {
                    return _root.Get<string>(EnvVars.ClientSecret);
                } 
            }
        }

        public class CspConfig
        {
            private IConfigurationRoot _root;
            public CspConfig(IConfigurationRoot root)
            {
                _root = root;
            }

            public string ScriptSrc
            {
                get
                {
                    return _root.Get<string>(EnvVars.CspScriptSource);
                }
            }

            public string StyleSrc 
            { 
                get 
                {
                    return _root.Get<string>(EnvVars.CspStyleSource);
                } 
            }

            public string FontSrc 
            { 
                get 
                {
                    return _root.Get<string>(EnvVars.CspFontSource);
                } 
            }

         }

    }
}
