using Fsw.Enterprise.AuthCentral.Areas.Admin.Models;
using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    public class ClientCreateModel : ClientModel
    {
        public ClientCreateModel() : base() { }

        public ClientCreateModel(Client client) : base(client)
        {
            SecretValue = client.ClientSecrets.FirstOrDefault()?.Value;
            SecretDescription = client.ClientSecrets.FirstOrDefault()?.Description;
            SecretExpiration = client.ClientSecrets.FirstOrDefault()?.Expiration;
            SecretType = client.ClientSecrets.FirstOrDefault()?.Type;
        }

        [Required]
        public string SecretValue { get; set; }
        [Required]
        public string SecretDescription { get; set; }
        [Required]
        public DateTimeOffset? SecretExpiration { get; set; }
        [Required]
        public string SecretType { get; set; }

        // calling this "ToNewClient" to clarify that this is only for clients while they are being created.
        // Normally you would not want to reinitialize the list of secrets.
        public Client ToNewClient()
        {
            Client client = base.ToClient();
            client.ClientSecrets = new List<Secret> {
                new ClientSecret
                {
                    Value = this.SecretValue,
                    Description = this.SecretDescription,
                    Expiration = this.SecretExpiration,
                    Type = this.SecretType
                }
            };
            return client;
        }
    }
}
