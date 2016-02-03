using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        
        /// <summary>
        /// Builds a new <see cref="IdentityServer3.Core.Models.Client"/> for use in client creation.
        /// This method will reset client secrets to only the one described in this object, and reset the flow to AuthorizationCode.
        /// To preserve the existing client's secret(s) and flow, use <see cref="ClientModel.ToClient"/> instead.
        /// </summary>
        /// <returns>A <see cref="IdentityServer3.Core.Models.Client"/> object with the relevant fields populated.</returns>
        public Client ToNewClient()
        {
            Client client = base.ToClient();
            client.Flow = Flows.AuthorizationCode;
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
