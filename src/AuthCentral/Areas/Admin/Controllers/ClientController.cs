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
        
        private const string pattern = @"^mongodb://.+?/(.+?)(?:\?(.+=.+)+$|$)";
        private static readonly Regex r = new Regex(pattern);

        private EnvConfig _cfg;
        private IClientService _clientService;
        private IScopeService _scopeService;

        //public ClientController(EnvConfig cfg, IAdminService adminService, IClientStore clientStore)
        public ClientController(EnvConfig cfg, IClientService clientService, IScopeService scopeService)
        {
            this._cfg = cfg;
            this._clientService = clientService;
            this._scopeService = scopeService;
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
            return View("Manage", client);
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> View(string id)
        {
            Client client = await _clientService.Find(id);

            //TODO: why is this method getting hit twice?
            //TODO: why is clientId always null?

            if(client != null)
            {
                return View(client);
            }
            else
            {
                ViewBag.Message = "The Auth Central Client with ClientId " + id + " could not be found.";
                return RedirectToAction("Index");
            }
        }


        // GET: /Client/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            Client client = await _clientService.Find(id);

            //TODO: why is this method getting hit twice?
            //TODO: why is clientId always null?

            if(client != null)
            {
                return View(client);
            }
            else
            {
                ViewBag.Message = "The Auth Central Client with ClientId " + id + " could not be found.";
                return RedirectToAction("Index");
            }
        }

        // GET: /Admin/Client/ViewClientSecrets/{clientId}
        [HttpGet]
        public async Task<IActionResult> ViewClientSecrets(string id)
        {
            Client client = await _clientService.Find(id);

            if(client == null)
            {
                ViewBag.Message = "The Auth Central Client with ClientId " + id + " could not be found.";
                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: /Admin/Client/CreateClientSecrets/{clientId}
        [HttpGet]
        public async Task<IActionResult> CreateClientSecret(string id)
        {
            Client client = await _clientService.Find(id);

            if(client == null)
            {
                ViewBag.Message = "The Auth Central Client with ClientId " + id + " could not be found.";
                return RedirectToAction("Index");
            }

            client.ClientSecrets.Add(new Secret());
            return View(client);
        }


        // POST: /Admin/Client/DeleteClientSecret
        [HttpPost]
        public async Task<IActionResult> DeleteClientSecret(string clientId, Secret clientSecret)
        {
            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = "The Auth Central Client with ClientId " + clientId + " could not be found.";
                return RedirectToAction("Index");
            }

            bool saveRequired = false;
            for(int i = (client.ClientSecrets.Count-1); i >= 0; i--)
            {
                var secret = client.ClientSecrets[i];

                if( secret.Value       == clientSecret.Value &&
                    secret.Description == clientSecret.Description &&
                    secret.Expiration  == clientSecret.Expiration &&
                    secret.Type        == clientSecret.Type )
                {
                    client.ClientSecrets.Remove(secret);
                    saveRequired = true;
                }
            }

            if(saveRequired)
            {
                _clientService.Save(client);
            }

            //            return RedirectToView("Edit", client);
            return RedirectToAction("Edit", new { id = client.ClientId } );
        }

        [HttpPost]
        public async Task<IActionResult> SaveClientSecret(string clientId, List<Secret> clientSecrets)
        {
            //TODO: validate??

            Client client = await _clientService.Find(clientId);

            if(client == null)
            {
                ViewBag.Message = "The Auth Central Client with ClientId " + clientId + " could not be found.";
                return RedirectToAction("Index");
            }

            foreach(var secret in clientSecrets)
            {
                // The value must be hashed to work with IdentityServer3
                secret.Value = secret.Value.Sha256();
                client.ClientSecrets.Add(secret);

            }

            await _clientService.Save(client);

            return RedirectToAction("Edit", new { id = client.ClientId } );
        }



        [HttpPost]
        public async Task<IActionResult> Save(Client client)
        {
            //TODO: Validation of some kind
            await _clientService.Save(client);

            ViewBag.Message = "The Auth Central Client '" + client.ClientName + "' was successfully saved!.";
            return View("Edit", client);
        }

    }
}
