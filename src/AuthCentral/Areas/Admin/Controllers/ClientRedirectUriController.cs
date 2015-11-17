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

namespace Fsw.Enterprise.AuthCentral.Areas.Admin
{
    [Area("Admin")]
    [Authorize("FswAdmin")]
    public class ClientRedirectUriController : Controller
    {
        
        private IClientService _clientService;

        //public ClientController(EnvConfig cfg, IAdminService adminService, IClientStore clientStore)
        public ClientRedirectUriController(IClientService clientService)
        {
            this._clientService = clientService;
        }

        [HttpGet]
        [Route("Admin/[controller]/[action]/{clientId}")]
        public async Task<IActionResult> Edit(string clientId)
        {
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return RedirectToAction("Edit");
            }

            var model = new ClientChildListContainer<string>()
            {
                ClientId = client.ClientId,
                ChildList = client.RedirectUris
            };
 
            model.ChildList.Add("");

            return View(model);
        }

        // POST: /Admin/Client/DeleteClientSecret
        [HttpPost]
        [Route("Admin/[controller]/[action]/{clientId}")]
        public async Task<IActionResult> Delete(string clientId, string redirectUri)
        {
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return RedirectToAction("Edit");
            }

            int removed = client.RedirectUris.RemoveAll(uri => uri.Equals(redirectUri));
            if (removed > 0) {
                await _clientService.Save(client);
            }

            var model = new ClientChildListContainer<string>()
            {
                ClientId = client.ClientId,
                ChildList = client.RedirectUris
            };
 
            model.ChildList.Add("");

            return View("Edit", model);
        }

        [HttpPost]
        [Route("Admin/[controller]/[action]/{clientId}")]
        public async Task<IActionResult> Save(string clientId, string redirectUri)
        {
            //TODO: validate??

            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
            }

            bool isSaveRequired = false;
            if(!client.RedirectUris.Contains(redirectUri) && !String.IsNullOrWhiteSpace(redirectUri))
            {
               client.RedirectUris.Add(redirectUri);
                isSaveRequired = true;
            }

            if(isSaveRequired)
            {
                await _clientService.Save(client);
            }

            var model = new ClientChildListContainer<string>()
            {
                ClientId = client.ClientId,
                ChildList = client.RedirectUris
            };

            // cheating way to include an empty for on the view page
            model.ChildList.Add("");
            
            return View("Edit", model);
        }

    }
}
