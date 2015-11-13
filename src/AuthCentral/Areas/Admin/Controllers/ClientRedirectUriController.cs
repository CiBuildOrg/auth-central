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
    public class ClientRedirectUriController : Controller
    {
        
        private EnvConfig _cfg;
        private IClientService _clientService;

        //public ClientController(EnvConfig cfg, IAdminService adminService, IClientStore clientStore)
        public ClientRedirectUriController(EnvConfig cfg, IClientService clientService)
        {
            this._cfg = cfg;
            this._clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string clientId)
        {
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = "The Auth Central Client with ClientId " + clientId + " could not be found.";
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
        public async Task<IActionResult> Delete(string clientId, string redirectUri)
        {
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = "The Auth Central Client with ClientId " + clientId + " could not be found.";
                return RedirectToAction("Edit");
            }

            bool saveRequired = false;
            for(int i = (client.RedirectUris.Count-1); i >= 0; i--)
            {
                string existingRedirecUri = client.RedirectUris[i];

                if(existingRedirecUri.Equals(redirectUri))
                {
                    client.RedirectUris.Remove(existingRedirecUri);
                    saveRequired = true;
                }
           }

            if(saveRequired)
            {
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
        public async Task<IActionResult> Save(string clientId, string redirectUri)
        {
            //TODO: validate??

            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = "The Auth Central Client with ClientId " + clientId + " could not be found.";
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
