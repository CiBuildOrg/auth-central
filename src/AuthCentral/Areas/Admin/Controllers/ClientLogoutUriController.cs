﻿using System;
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

namespace Fsw.Enterprise.AuthCentral.Areas.Admin
{
    [Area("Admin")]
    [Authorize("FswAdmin")]
    public class ClientLogoutUriController : Controller
    {
        
        private IClientService _clientService;

        public ClientLogoutUriController(IClientService clientService)
        {
            this._clientService = clientService;
        }

        [HttpGet]
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
                ChildList = client.PostLogoutRedirectUris
            };
 
            model.ChildList.Add("");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string clientId, string postLogoutUri)
        {
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return RedirectToAction("Edit");
            }

            int removed = client.PostLogoutRedirectUris.RemoveAll(uri => uri.Equals(postLogoutUri));
            if (removed > 0) {
                await _clientService.Save(client);
            }

            var model = new ClientChildListContainer<string>()
            {
                ClientId = client.ClientId,
                ChildList = client.PostLogoutRedirectUris
            };
 
            model.ChildList.Add("");

            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(string clientId, string redirectUri)
        {
            //TODO: validate??

            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
            }

            bool isSaveRequired = false;
            if(!client.PostLogoutRedirectUris.Contains(redirectUri) && !String.IsNullOrWhiteSpace(redirectUri))
            {
               client.PostLogoutRedirectUris.Add(redirectUri);
                isSaveRequired = true;
            }

            if(isSaveRequired)
            {
                await _clientService.Save(client);
            }

            var model = new ClientChildListContainer<string>()
            {
                ClientId = client.ClientId,
                ChildList = client.PostLogoutRedirectUris
            };

            // cheating way to include an empty for on the view page
            model.ChildList.Add("");
            
            return View("Edit", model);
        }

    }
}
