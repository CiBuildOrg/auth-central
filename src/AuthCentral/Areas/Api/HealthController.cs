using System;
using System.IO;
using Fsw.Enterprise.AuthCentral.Health;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using Newtonsoft.Json;

namespace Fsw.Enterprise.AuthCentral.Controllers
{
    [Area("Api")]
    [Route("[controller]"), Route("[area]/[controller]")]
    public class HealthController : Controller
    {
        private ILogger _logger;
        private static ProjectInfo _project;

        
        static HealthController()
        {
            if (System.IO.File.Exists("src/AuthCentral/project.json"))
            {
                _project = JsonConvert.DeserializeObject<ProjectInfo>(System.IO.File.ReadAllText("src/AuthCentral/project.json"));
            }
            else if (System.IO.File.Exists("project.json"))
            {
                _project = JsonConvert.DeserializeObject<ProjectInfo>(System.IO.File.ReadAllText("project.json"));
            }
            else
            {
                _project = new ProjectInfo()
                {
                    Name = "AuthCentral",
                    Version = "unknown",
                    Commit = "unknown"
                };
            }
        }

        public HealthController(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger(this.GetType().ToString());
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