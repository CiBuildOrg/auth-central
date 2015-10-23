using Microsoft.Framework.Configuration;
using System;

namespace Fsw.Enterprise.AuthCentral
{
    internal class EnvConfig
    {
        private CertConfig _cert;
        private UriConfig _uri;
        private DatabaseConfig _db;
        private IConfigurationRoot _root;

        private static class EnvVars {
            public static string DbMembershipReboot = "AUTHCENTRAL_DB_MEMBERSHIPREBOOT";
            public static string DbIdentityServer3 = "AUTHCENTRAL_DB_IDENTITYSERVER3";
            public static string UriScheme = "AUTHCENTRAL_URI_SCHEME";
            public static string UriHost = "AUTHCENTRAL_URI_HOST";
            public static string UriPort = "AUTHCENTRAL_URI_PORT";
            public static string UriServiceRoot = "AUTHCENTRAL_URI_SERVICEROOT";
            public static string CertStoreName = "AUTHCENTRAL_CERT_STORENAME";
            public static string CertThumbprint = "AUTHCENTRAL_CERT_THUMBPRINT";
        }

        public EnvConfig(IConfigurationRoot root) {
            this._root = root;
            this._cert = new CertConfig(root);
            this._uri = new UriConfig(root);
            this._db = new DatabaseConfig(root);
        }

        public CertConfig Cert {
            get { return this._cert; }
        }

        public DatabaseConfig DB {
            get { return this._db; }
        }

        public UriConfig Uri {
            get { return this._uri; }
        }

        internal class DatabaseConfig
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


        internal class UriConfig
        {
            private IConfigurationRoot _root;
            public UriConfig(IConfigurationRoot root)
            {
                _root = root;
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

            public string ServiceRoot 
            { 
                get 
                {
                    return _root.Get<string>(EnvVars.UriServiceRoot);
                } 
            }

            public string IssuerUri
            {
                get
                {
                    return new UriBuilder(this.Scheme, this.Host, this.Port, this.ServiceRoot).Uri.AbsoluteUri;
                }
            }

        }


        internal class CertConfig
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
           
        private static string GetEnvironmentVariable(string name, string valueDefault)
        {
            try
            {
                var result = System.Environment.GetEnvironmentVariable(name);
                if (result == null)
                {
                    return valueDefault;
                }
                else
                {
                    return result;
                }
            }
            catch (System.ArgumentNullException)
            {
                return valueDefault;
            }
        }
    }
}
