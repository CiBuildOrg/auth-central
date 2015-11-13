using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Microsoft.AspNet.Mvc;

using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

using Fsw.Enterprise.AuthCentral.ViewModels;
using Fsw.Enterprise.AuthCentral.MongoStore.Admin;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin
{
    [Area("Admin")]
    public class ClientController : Controller
    {
        private EnvConfig _cfg;
        private IClientService _clientService;

        //public ClientController(EnvConfig cfg, IAdminService adminService, IClientStore clientStore)
        public ClientController(EnvConfig cfg, IClientService clientService)
        {
            this._cfg = cfg;
            this._clientService = clientService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/
        [HttpGet]
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

            //return RedirectToAction("Manage", client);
            return View("Edit", client);
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> View(string clientId)
        {
            Client client = await _clientService.Find(clientId);

            //TODO: why is this method getting hit twice?
            //TODO: why is clientId always null?

            if(client != null)
            {
                return View(client);
            }
            else
            {
                ViewBag.Message = "The Auth Central Client with ClientId " + clientId + " could not be found.";
                return View("Index", ViewBag);
            }
        }


        // GET: /Client/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string clientId)
        {
            Client client = await _clientService.Find(clientId);

            if(client != null)
            {
                return View(client);
            }
            else
            {
                ViewBag.Message = "The Auth Central Client with ClientId " + clientId + " could not be found.";
                return View("Index", ViewBag);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string clientId)
        {
            await _clientService.Delete(clientId);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Save(Client client)
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

            ViewBag.Message = "The Auth Central Client '" + client.ClientName + "' was successfully saved!.";
            return RedirectToAction("Edit", new { clientId = client.ClientId });
//            return View("Edit", client);
        }

    }
}
