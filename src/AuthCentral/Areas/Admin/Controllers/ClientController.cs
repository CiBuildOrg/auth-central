using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

using Fsw.Enterprise.AuthCentral.MongoStore;
using Fsw.Enterprise.AuthCentral.MongoStore.Admin;
using Fsw.Enterprise.AuthCentral.Areas.Admin.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Controllers
{
    [Authorize("FswAdmin")]
    [Area("Admin"), Route("[area]/[controller]")]
    public class ClientController : Controller
    {
        private const string CLIENT_COUNT_COOKIE_KEY = "idsrv.admin.clients.count";
        private IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            this._clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            ClientPagingResult clientsPage = await this._clientService.GetPageAsync(page, pageSize);
            ClientListViewModel clientListViewModel = new ClientListViewModel(clientsPage, page, pageSize);
            int clientsFoundForPage = clientListViewModel.Clients.ToList().Count;

            // retrieve the stored item count (if it exists)
            int itemCount = 0;
            string itemCountAsString = this.Request.Cookies[CLIENT_COUNT_COOKIE_KEY];


            // create the ClientListViewModel with appropriate total item account
            if (itemCountAsString != null && int.TryParse(itemCountAsString, out itemCount) && 
                itemCount > clientListViewModel.TotalItemCount && clientsFoundForPage > 0 && 
                clientsPage.HasMore)
            {
                // use the saved total item counts
                clientListViewModel = new ClientListViewModel(clientsPage, page, pageSize, itemCount);
            }

            if(itemCount != clientListViewModel.TotalItemCount && clientsFoundForPage > 0)
            {
                this.Response.Cookies.Append(CLIENT_COUNT_COOKIE_KEY, clientListViewModel.TotalItemCount.ToString());
            }

            return View(clientListViewModel);
        }

        [HttpGet("[action]")]
        public IActionResult Create()
        {
            var client = new Client() {
                UpdateAccessTokenClaimsOnRefresh = true,
                PrefixClientClaims = false,
                AlwaysSendClientClaims = true,
                RequireConsent = false,
                LogoUri = "https://img3.foodservicewarehouse.com/Img/fsw15.svg",
                Flow = Flows.AuthorizationCode
            };

            return View("Edit", client);
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            Client existingClient = await _clientService.Find(client.ClientId);

            if(existingClient == null)
            {
                return await this.Save(client);
            }
            else
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} already exists.  Please use a different and unique clientId.", client.ClientId);
                return View("Edit", client);
            }
 
        }


        [HttpGet("[action]/{clientId?}")]
        public async Task<IActionResult> Edit(string clientId)
        {
            Client client = await _clientService.Find(clientId);

            if(client != null)
            {
                return View(client);
            }
            else
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return View("Index", ViewBag);
            }
        }

        [HttpPost("[action]/{clientId?}")]
        public async Task<IActionResult> Disable(string clientId, int page = 1)
        {
            Client client = await _clientService.Find(clientId);

            if(client != null)
            {
                client.Enabled = false;
                await _clientService.Save(client);

                return this.RedirectToAction("Index", "Client", new
                {
                    page = page
                });
            }
            else
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return View("Index");
            }
        }

        [HttpPost("[action]/{clientId?}")]
        public async Task<IActionResult> Enable(string clientId, int page = 1)
        {
            Client client = await _clientService.Find(clientId);

            if(client != null)
            {
                client.Enabled = true;
                await _clientService.Save(client);

                return RedirectToAction("Index", "Client", new
                {
                    page = page
                });
            }
            else
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return View("Index");
            }
        }

        [HttpPost("[action]/{clientId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string clientId)
        {
            await _clientService.Delete(clientId);

            return RedirectToAction("Index");
        }


        [HttpPost("[action]/{clientId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Client client)
        {
            if(ModelState.IsValid)
            {
                var existingClient = await _clientService.Find(client.ClientId);
                if(existingClient != null)
                {
                    // don't overwrite existing child items not part of the passed in client
                    client.AllowedCorsOrigins = existingClient.AllowedCorsOrigins;
                    client.AllowedCustomGrantTypes = existingClient.AllowedCustomGrantTypes;
                    client.AllowedScopes = existingClient.AllowedScopes;
                    client.Claims = existingClient.Claims;
                    client.ClientSecrets = existingClient.ClientSecrets;
                    client.IdentityProviderRestrictions = existingClient.IdentityProviderRestrictions;
                    client.PostLogoutRedirectUris = existingClient.PostLogoutRedirectUris;
                    client.RedirectUris = existingClient.RedirectUris;
                }
                else
                {
                    // set some FSW defaults for the new client
                    var defaultScopes = new List<string>();
                    defaultScopes.Add("openid");
                    defaultScopes.Add("profile");
                    defaultScopes.Add("offline_access");
                    defaultScopes.Add("fsw_platform");
                    client.AllowedScopes = defaultScopes;
                }

                if (client.ClientId == null)
                {
                    // default to client name and ID being the same
                    client.ClientId = client.ClientName;
                } 

                await _clientService.Save(client);
            }

            ViewBag.Message = string.Format("The Auth Central Client {0} was successfully saved!", client.ClientName);
            return RedirectToAction("Edit", new { clientId = client.ClientId, ViewBag = ViewBag });
        }

    }
}
