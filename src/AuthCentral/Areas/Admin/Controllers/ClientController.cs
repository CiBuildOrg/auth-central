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
        private const int DEFAULT_PAGE_SIZE = 10;
        private const string CLIENT_COUNT_COOKIE_KEY = "idsrv.admin.clients.count";
        private IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            this._clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = DEFAULT_PAGE_SIZE)
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
                LogoUri = "https://fsw-res-1.cloudinary.com/d_noimage.jpg,h_69,w_160,c_fill/logos/fsw-logo.svg",
                Flow = Flows.AuthorizationCode
            };

            var secret = new ClientSecret();
            secret.Expiration = new DateTimeOffset(DateTime.UtcNow.AddYears(1));
            client.ClientSecrets.Add(secret);
            return View(client);
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
        public async Task<IActionResult> Edit(string clientId, bool success=false)
        {
            Client client = await _clientService.Find(clientId);

            if(client != null)
            {
                if(success)
                {
                    ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} saved succesfully.", clientId);
                }

                return View(client);
            }
            else
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return View("Index", ViewBag);
            }
        }

        [HttpPost("[action]/{clientId?}")]
        public async Task<IActionResult> Disable(string clientId, int page = 1, int pageSize = DEFAULT_PAGE_SIZE)
        {
            Client client = await _clientService.Find(clientId);

            if(client != null)
            {
                client.Enabled = false;
                await _clientService.Save(client);

                return this.RedirectToAction("Index", "Client", new
                {
                    page = page,
                    pageSize = pageSize
                });
            }
            else
            {
                ViewBag.Message = string.Format("The Auth Central Client with ClientId {0} could not be found.", clientId);
                return View("Index");
            }
        }

        [HttpPost("[action]/{clientId?}")]
        public async Task<IActionResult> Enable(string clientId, int page = 1, int pageSize = DEFAULT_PAGE_SIZE)
        {
            Client client = await _clientService.Find(clientId);

            if(client != null)
            {
                client.Enabled = true;
                await _clientService.Save(client);

                return RedirectToAction("Index", "Client", new
                {
                    page = page,
                    pageSize = pageSize
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


        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Client client)
        {
            if(ModelState.IsValid)
            {
                await PersistClient(client);

                return RedirectToAction("Edit", new { clientId = client.ClientId, success = true });
            }
            else
            {
                ViewBag.Message = string.Format("ModelState is not valid. Try again or something");
                return View("Edit", client);
            }
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew(Client client)
        {
            if(ModelState.IsValid)
            {

                await PersistClient(client);

                return RedirectToAction("Edit", new { clientId = client.ClientId });
            }
            else
            {
                ViewBag.Message = string.Format("ModelState is not valid. Try again or something");
                return View("Create", client);
            }
        }


        private async Task PersistClient(Client client)
        {
            if (client.ClientId == null)
            {
                // default to client name and ID being the same
                client.ClientId = client.ClientName;
            } 

            var existingClient = await _clientService.Find(client.ClientId);
            if(existingClient != null)
            {
                await UpdateExistingClient(client, existingClient);
            }
            else
            {
                await CreateNewClient(client);
            }
        }

        private async Task UpdateExistingClient(Client modifiedClient, Client existingClient)
        {
            // don't overwrite existing child items not part of the passed in client
            modifiedClient.AllowedCorsOrigins = existingClient.AllowedCorsOrigins;
            modifiedClient.AllowedCustomGrantTypes = existingClient.AllowedCustomGrantTypes;
            modifiedClient.AllowedScopes = existingClient.AllowedScopes;
            modifiedClient.Claims = existingClient.Claims;
            modifiedClient.ClientSecrets = existingClient.ClientSecrets;
            modifiedClient.IdentityProviderRestrictions = existingClient.IdentityProviderRestrictions;
            modifiedClient.PostLogoutRedirectUris = existingClient.PostLogoutRedirectUris;
            modifiedClient.RedirectUris = existingClient.RedirectUris;

            await _clientService.Save(modifiedClient);
        }

        private async Task CreateNewClient(Client newClient)
        {
            // set some FSW defaults for the new client
            var defaultScopes = new List<string>();
            defaultScopes.Add("openid");
            defaultScopes.Add("profile");
            defaultScopes.Add("offline_access");
            defaultScopes.Add("fsw_platform");
            newClient.AllowedScopes = defaultScopes;

            if(!String.IsNullOrWhiteSpace(newClient.ClientSecrets[0].Value))
            {
                // per spec, client passwords must be hashed using SHA256
                // the secret provided on the new client creation form has
                // not been hashed yet, so we need to do it now before we
                // store the client with all of it's child elements
                newClient.ClientSecrets[0].Value = newClient.ClientSecrets[0].Value.Sha256();
            }

            // don't save empty redirect uri's
            int count = newClient.RedirectUris.Count;
            if(count > 0) {
                for(int i = count-1; i>=0; i--)
                {
                    if(String.IsNullOrWhiteSpace(newClient.RedirectUris[i]))
                    {
                        newClient.RedirectUris.RemoveAt(i);
                    }
                }
            }

            // don't save empty postLogoutRedirect uri's
            count = newClient.PostLogoutRedirectUris.Count;
            if(count > 0) {
                for(int i = count-1; i>=0; i--)
                {
                    if(String.IsNullOrWhiteSpace(newClient.PostLogoutRedirectUris[i]))
                    {
                        newClient.PostLogoutRedirectUris.RemoveAt(i);
                    }
                }
            }


            await _clientService.Save(newClient);
        }
    }
}
