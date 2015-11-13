using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Microsoft.AspNet.Mvc;

using IdentityServer3.Core.Models;

using Fsw.Enterprise.AuthCentral.ViewModels;
using Fsw.Enterprise.AuthCentral.MongoStore;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin
{
    [Area("Admin")]
    [Authorize("FswAdmin")]
    public class AccountController : Controller
    {
        EnvConfig _cfg;

        public AccountController(EnvConfig cfg)
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
        public IActionResult Index(AuthCentralClientViewModel model)
        {
           return View("Index", model);
        }

    }
}
