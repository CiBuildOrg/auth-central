using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Fsw.Enterprise.AuthCentral
{
    public class EnvConfig
    {
        private DpConfig _dp;
        private UriConfig _uri;
        private CspConfig _csp;
        private CertConfig _cert;
        private SmtpConfig _smtp;
        private DatabaseConfig _db;
        private ClientConfig _client;
        private IConfigurationRoot _root;

        private static class EnvVars {
            public static string DpSharedKeystoreDir = "AUTHCENTRAL_DP_SHARED_KEYSTORE_DIR";
            public static string DpCertThumbprint = "AUTHCENTRAL_DP_CERT_THUMBPRINT";
            public static string JwksCertStore = "AUTHCENTRAL_JWKS_CERT_STORENAME";
            public static string JwksCertThumbprint = "AUTHCENTRAL_JWKS_CERT_THUMBPRINT";
            public static string JwksSecondaryCertStore = "AUTHCENTRAL_JWKS_CERT2_STORENAME";
            public static string JwksSecondaryCertThumbprint = "AUTHCENTRAL_JWKS_CERT2_THUMBPRINT";
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
            this._dp = new DpConfig(root, this._uri, AppName);
        }

        public string AppName
        {
            get { return "FSW Auth Central"; }
        }

        public CspConfig Csp {
            get { return this._csp; }
        }

        public CertConfig Cert {
            get { return this._cert; }
        }

        public DpConfig DataProtection {
            get { return this._dp; }
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

            public string JwksCertStoreName
            {
                get
                {
                    return _root.Get<string>(EnvVars.JwksCertStore);
                }
            }

            public string JwksCertThumbprint 
            { 
                get 
                {
                    return _root.Get<string>(EnvVars.JwksCertThumbprint);
                } 
            }
            public string JwksSecondaryCertStoreName
            {
                get
                {
                    return _root.Get<string>(EnvVars.JwksSecondaryCertStore);
                }
            }

           public string JwksSecondaryCertThumbprint 
            { 
                get 
                {
                    return _root.Get<string>(EnvVars.JwksSecondaryCertThumbprint);
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
        public class DpConfig
        {
            private IConfigurationRoot _root;
            private string _appName;
            private UriConfig _uriConfig;

            internal DpConfig(IConfigurationRoot root, UriConfig uriConfig, string appName)
            {
                _root = root;
                _appName = appName;
                _uriConfig = uriConfig;
            }

            public string AppName {
                get
                {
                    return _appName;
                }
            }

            public string SharedKeystoreDir
            { 
                get 
                {
                    // Use in a directory specific to the thumbprint being used
                    return Path.Combine(_root.Get<string>(EnvVars.DpSharedKeystoreDir), _uriConfig.Host, this.CertThumbprint);
                } 
            }

            public string CertStoreName
            { 
                get 
                {
                    // must be installed in the personal cert store on the
                    // local machine
                    return "MY";
                } 
            }

            public string CertThumbprint
            { 
                get 
                {
                    // must be installed in the personal cert store on the
                    // local machine
                    return _root.Get<string>(EnvVars.DpCertThumbprint);
                } 
            }


         }


    }
}
