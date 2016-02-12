using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNet.DataProtection.SystemWeb;
using Microsoft.AspNet.DataProtection.Repositories;
using Microsoft.AspNet.DataProtection.KeyManagement;
using Microsoft.AspNet.DataProtection;

namespace Fsw.Enterprise.AuthCentral.Crypto
{
    public class AuthCentralDataProtectionStartup: DataProtectionStartup
    {
        private static EnvConfig _config;
        internal static EnvConfig AuthCentralEnvConfig
        {
            get
            {
                if(_config == null)
                {
                    throw new NullReferenceException("Static Member AuthCentralEnvConfig has not yet been set.  This field must be set before it can be retrieved.");
                }
                return _config;
            }
            set
            {
                _config = value;
            }
        }

        public override void ConfigureServices(IServiceCollection services)
        {
//            services.AddSingleton<IXmlRepository>(xmlRepo =>
//            {
//                var appDir = new DirectoryInfo(Environment.CurrentDirectory);
//                return new FileSystemXmlRepository(appDir);
//            });
//            KeyManagementOptions kmo = new KeyManagementOptions();
//            kmo.AutoGenerateKeys = false;
//            kmo.
//            EnvConfig config = services.

            // we do this to ensure cookie and anti-forgery keys can be
            // encrypted/decrypted on any machine in the cluster to allow
            // for stateless loadbalancing algorithms (which scale better, 
            // balance more evenly, and make deployments easier).
            services.ConfigureDataProtection(GetConfiguration(AuthCentralEnvConfig));
        }

        public static Action<DataProtectionConfiguration> GetConfiguration(EnvConfig config)
        {
            // make sure the shared keystore directory exists
            if(!Directory.Exists(config.DataProtection.SharedKeystoreDir))
            {
                Directory.CreateDirectory(config.DataProtection.SharedKeystoreDir);
            }

            return new Action<DataProtectionConfiguration>(dataProtectionConfiguration =>
            {
                dataProtectionConfiguration.SetApplicationName(config.DataProtection.AppName);
                dataProtectionConfiguration.PersistKeysToFileSystem(new DirectoryInfo(config.DataProtection.SharedKeystoreDir));
                dataProtectionConfiguration.ProtectKeysWithCertificate(config.DataProtection.CertificateThumbprint);
            });
        }
    }

}
