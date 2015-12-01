using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Microsoft.AspNet.Mvc;

using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

using Fsw.Enterprise.AuthCentral.Areas.Admin.Models;
using Fsw.Enterprise.AuthCentral.MongoStore.Admin;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Controllers
{
    [Authorize("FswAdmin")]
    [Area("Admin"), Route("[area]/[controller]")]
    public class ClientSecretController : ClientAdminController
    {
        private IClientService _clientService;

        public ClientSecretController(IClientService clientService)
        {
            this._clientService = clientService;
        }

        [HttpGet("[action]/{clientId}")]
        public async Task<IActionResult> Show(string clientId)
        {
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return RedirectToAction("Index");
            }

            var secrets = new List<ClientSecret>();
            foreach(var secret in client.ClientSecrets)
            {
                secrets.Add(new ClientSecret(secret));
            }

            var model = new ClientSecretContainer()
            {
                ClientId = client.ClientId,
                ClientSecrets = secrets
            };
            
            return View(model);
        }

        [HttpGet("[action]/{clientId}")]
        public async Task<IActionResult> Create(string clientId)
        {
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return RedirectToAction("Index");
            }

            var model = new ClientSecretContainer()
            {
                ClientId = client.ClientId,
                ClientSecrets = new List<ClientSecret>()
            };

            model.ClientSecrets.Add(new ClientSecret(new Secret()));

 
            return View(model);
        }


        [HttpPost("[action]/{clientId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string clientId, ClientSecret clientSecret)
        {
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return RedirectToAction("Index");
            }

            bool saveRequired = false;
            for(int i = (client.ClientSecrets.Count-1); i >= 0; i--)
            {
                var existingSecret = client.ClientSecrets[i];

                if( (existingSecret.Value       == clientSecret.Value &&
                     existingSecret.Description == clientSecret.Description &&
                     existingSecret.Expiration  == clientSecret.Expiration &&
                     existingSecret.Type        == clientSecret.Type
                    ) ||
                    String.IsNullOrWhiteSpace(existingSecret.Value) && String.IsNullOrWhiteSpace(clientSecret.Value)
                ){
                    client.ClientSecrets.Remove(existingSecret);
                    saveRequired = true;
                    break;
                }
           }

            if(saveRequired)
            {
                await _clientService.Save(client);
            }

            return RedirectToAction("Show", new { clientId = client.ClientId } );
        }

        [HttpPost("[action]/{clientId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(string clientId, List<ClientSecret> clientSecrets)
        {
            //TODO: validate??
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", client.ClientId);
                return RedirectToAction("Index");
            }

            bool saveRequired = false;
            foreach(ClientSecret cs in clientSecrets)
            {
                // The value must be hashed to work with IdentityServer3
                if(!String.IsNullOrWhiteSpace(cs.Value))
                {
                    cs.Value = cs.Value.Sha256();
                    client.ClientSecrets.Add(cs);

                    saveRequired = true;
                }
            }

            if(saveRequired)
            {
                await _clientService.Save(client);
            }

            return RedirectToAction("Show", new { clientId = client.ClientId } );
        }

    }
}
