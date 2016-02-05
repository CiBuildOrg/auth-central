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
        [Required,Url]
        public string ClientUri { get; set; }
        [Required]
        public string LogoUri { get; set; }

        public Client ExistingClient { get; set; }

        public ClientModel() : base() {
            ExistingClient = new Client();
        }
        
        public ClientModel(Client client) : base()
        {
            ExistingClient = client ?? new Client();
            ClientId = client.ClientId;
            ClientName = client.ClientName;
            ClientUri = client.ClientUri;
            LogoUri = client.LogoUri;
        }

        public virtual Client ToClient()
        {
            if(ExistingClient == null)
            {
                ExistingClient = new Client();
            }
            ExistingClient.ClientId = this.ClientId;
            ExistingClient.ClientName = this.ClientName;
            ExistingClient.ClientUri = this.ClientUri;
            ExistingClient.LogoUri = this.LogoUri;
            return ExistingClient;
        }
    }
}
