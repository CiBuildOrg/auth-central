using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IdentityServer3.Core.Models;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    public class ClientSecretContainer
    {
        public string ClientId { get; set; }
        public List<ClientSecret> ClientSecrets { get; set; }

    }

    public class ClientSecret: Secret
    {
        public ClientSecret() { }
        public ClientSecret(Secret secret)
        {
            this.Value = secret.Value;
            this.Description = secret.Description;
            this.Expiration = secret.Expiration;
        }

    }
}
