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
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin
{
    [Area("Admin")]
//    [Authorize("FswAdmin")]
    public class ClientClaimController : Controller
    {
        private IClientService _clientService;

        public ClientClaimController(IClientService clientService)
        {
            this._clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> Show(string clientId)
        {
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return RedirectToAction("Index");
            }

            var clientClaims = new List<ClientClaim>();
            foreach(Claim claim in client.Claims)
            {
                clientClaims.Add(new ClientClaim(claim));
            }

            var model = new ClientClaimContainer()
            {
                ClientId = client.ClientId,
                ClientClaims = clientClaims
            };
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string clientId)
        {
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return RedirectToAction("Index");
            }

            var model = new ClientClaimContainer()
            {
                ClientId = client.ClientId,
                ClientClaims = new List<ClientClaim>()
            };

            model.ClientClaims.Add(new ClientClaim());
 
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string clientId, ClientClaim clientClaim)
        {
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return RedirectToAction("Index");
            }

            bool saveRequired = false;
            for(int i = (client.Claims.Count-1); i >= 0; i--)
            {
                var existingClientClaim = client.Claims[i];

                if( existingClientClaim.Value       == clientClaim.Value &&
                    existingClientClaim.Type        == clientClaim.Type )
                {
                    client.Claims.Remove(existingClientClaim);
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

        [HttpPost]
        public async Task<IActionResult> Save(ClientClaimContainer csc)
        {
            //TODO: validate??

            Client client = await _clientService.Find(csc.ClientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", client.ClientId);
                return RedirectToAction("Index");
            }

            foreach(var clientClaim in csc.ClientClaims)
            {
                client.Claims.Add(new Claim(clientClaim.Type, clientClaim.Value));
            }

            await _clientService.Save(client);
            return RedirectToAction("Show", new { clientId = client.ClientId } );
        }

    }
}
