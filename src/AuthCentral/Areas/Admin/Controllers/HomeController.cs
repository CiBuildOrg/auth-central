using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Microsoft.AspNet.Mvc;

using IdentityServer3.Core.Models;

using Fsw.Enterprise.AuthCentral.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        EnvConfig _cfg;

        public HomeController(EnvConfig cfg)
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
