using System;
using System.Reflection;
using Fsw.Enterprise.AuthCentral.Health;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Controllers
{
    [Route("[controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        public StatusResource Health()
        {
            var status = new StatusResource();
            status.AppName = "AuthCentral";
            status.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            status.Status = HealthContext.CurrentStatus;

            return status;
        }

        public class StatusResource
        {
            public string AppName { get; set; }
            public string Version { get; set; }
            public string Status { get; set; }
            public string Commit { get; set; }
        }

    }
}