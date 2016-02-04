using IdentityServer3.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    public class ClientModel
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ClientName { get; set; }
        [Required]
        public string ClientUri { get; set; }
        [Required]
        public string LogoUri { get; set; }

        public string RedirectUri { get; set; }
        public string PostLogoutUri { get; set; }

        public Client ExistingClient { get; set; }

        public ClientModel() : base() { }
        
        public ClientModel(Client client) : this()
        {
            ExistingClient = client;
            ClientId = client.ClientId;
            ClientName = client.ClientName;
            ClientUri = client.ClientUri;
            LogoUri = client.LogoUri;
            RedirectUri = client.RedirectUris.FirstOrDefault();
            PostLogoutUri = client.PostLogoutRedirectUris.FirstOrDefault();
        }

        public virtual Client ToClient()
        {
            ExistingClient.ClientId = this.ClientId;
            ExistingClient.ClientName = this.ClientName;
            ExistingClient.ClientUri = this.ClientUri;
            ExistingClient.LogoUri = this.LogoUri;
            return ExistingClient;
        }
    }
}
