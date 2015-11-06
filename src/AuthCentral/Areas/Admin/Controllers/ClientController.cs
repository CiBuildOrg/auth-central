using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Microsoft.AspNet.Mvc;

using IdentityServer3.Admin.MongoDb;
using IdentityServer3.MongoDb;
using IdentityServer3.Core.Models;

using Fsw.Enterprise.AuthCentral.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin
{
    [Area("Admin")]
    public class ClientController : Controller
    {
        private const string pattern = @"^mongodb://.+?/(.+?)(?:\?(.+=.+)+$|$)";
        private static readonly Regex r = new Regex(pattern);
        EnvConfig _cfg;

        public ClientController(EnvConfig cfg)
        {
            this._cfg = cfg; 
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(AuthCentralClientViewModel model)
        {
            var settings = StoreSettings.DefaultSettings();
            settings.ConnectionString = _cfg.DB.IdentityServer3;
            settings.Database = getDbNameFromMongoConnectionString(settings.ConnectionString);

            IAdminService adminSvc = AdminServiceFactory.Create(settings);

            var idsrvrClient = new Client();

            await adminSvc.Save(idsrvrClient);

            ViewBag.Message = "The Auth Central Client " + model.Name + " was successfully saved!.";
            return View("Index", model);
        }
        private static string getDbNameFromMongoConnectionString(string connectionString)
        {
            string result = "identityserver";

            Match match = r.Match(connectionString);

            if(match.Success)
            {
                result = match.Groups[1].Value;
            }

            return result;
        }

    }
}
