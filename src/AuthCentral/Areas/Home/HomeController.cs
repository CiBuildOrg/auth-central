using System;
using System.IO;
using Fsw.Enterprise.AuthCentral.Health;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fsw.Enterprise.AuthCentral.Controllers
{
    [Route("")]
    public class IndexController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return this.RedirectPermanent("/ids");
        }
    }
}