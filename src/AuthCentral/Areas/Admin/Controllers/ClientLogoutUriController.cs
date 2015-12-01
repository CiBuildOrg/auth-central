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

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Controllers
{
    [Authorize("FswAdmin")]
    [Area("Admin"), Route("[area]/[controller]")]
    public class ClientLogoutUriController : Controller
    {
        
        private IClientService _clientService;

        public ClientLogoutUriController(IClientService clientService)
        {
            this._clientService = clientService;
        }

        [HttpGet("[action]/{clientId}")]
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

        [HttpPost("[action]/{clientId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string clientId, string postLogoutUri)
        {
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return RedirectToAction("Edit");
            }

            if(client.PostLogoutRedirectUris.Contains(postLogoutUri) )
            {
                client.PostLogoutRedirectUris.Remove(postLogoutUri);
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

        [HttpPost("[action]/{clientId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(string clientId, string originalPostLogoutUri, string postLogoutUri)
        {
            //TODO: validate??

            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
            }

            // if things have changed
            if (originalPostLogoutUri != postLogoutUri)
            {
                bool saveRequired = false;

                if (!client.PostLogoutRedirectUris.Contains(postLogoutUri) && !String.IsNullOrWhiteSpace(postLogoutUri))
                {
                    var insertAt = client.PostLogoutRedirectUris.IndexOf(originalPostLogoutUri);
                    if (insertAt >= 0)
                    {
                        client.PostLogoutRedirectUris.Insert(insertAt, postLogoutUri);
                    }
                    else
                    {
                        client.PostLogoutRedirectUris.Add(postLogoutUri);
                    }
                    saveRequired = true;
                }

                if (client.PostLogoutRedirectUris.Contains(originalPostLogoutUri))
                {
                    client.PostLogoutRedirectUris.Remove(originalPostLogoutUri);
                    saveRequired = true;
                }

                if (saveRequired)
                {
                    await _clientService.Save(client);
                }
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
