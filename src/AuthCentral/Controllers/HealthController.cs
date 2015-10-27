using System;
using System.Reflection;
using Fsw.Enterprise.AuthCentral.Health;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using Newtonsoft.Json;

namespace Fsw.Enterprise.AuthCentral.Controllers
{
    [Route("[controller]")]
    public class HealthController : Controller
    {
        private ILogger _logger;
        private ProjectInfo _project;
        public HealthController(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger("Fsw.Enterprise.AuthCentral.Controllers.HealthController");
            _project = JsonConvert.DeserializeObject<ProjectInfo>(System.IO.File.ReadAllText("project.json"));
        }

        [HttpGet]
        public StatusResource Health()
        {
            var status = new StatusResource();
            status.AppName = _project.Name;
            status.Version = _project.Version;
            status.Status = HealthContext.CurrentStatus;
            status.Commit = _project.Commit;
            return status;
        }

        public class StatusResource
        {
            public string AppName { get; set; }
            public string Version { get; set; }
            public string Status { get; set; }
            public string Commit { get; set; }
        }

        internal class ProjectInfo
        {
            public string Name;
            public string Commit;
            public string Version;
        }
    }
}