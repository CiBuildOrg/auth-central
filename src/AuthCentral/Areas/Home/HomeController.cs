using System;
using System.IO;
using Fsw.Enterprise.AuthCentral.Health;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fsw.Enterprise.AuthCentral.Controllers
{
    [Area("Home"), Route("")]
    public class IndexController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return this.RedirectPermanent("/useraccount/profile");
        }
    }
}