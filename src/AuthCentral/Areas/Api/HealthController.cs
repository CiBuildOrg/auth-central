using System;
using System.IO;
using System.Collections.Generic;
using Fsw.Enterprise.AuthCentral.Health;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNet.Authorization;

namespace Fsw.Enterprise.AuthCentral.Areas.Api.Controllers
{
    [Area("Api")]
    [Route("[area]"), Route("")]
    public class HealthController : Controller
    {
        private ILogger _logger;
        private IApplicationEnvironment _app;
        private IRuntimeEnvironment _runtime;
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

        public HealthController(ILoggerFactory factory, IApplicationEnvironment app, IRuntimeEnvironment runtime)
        {
            _logger = factory.CreateLogger(this.GetType().ToString());
            _app = app;
            _runtime = runtime;
        }

        [HttpGet("[action]")]
        public HealthResource Health()
        {
            var status = new HealthResource();
            status.AppName = _project.Name;
            status.Version = _project.Version;
            status.Status = HealthContext.CurrentStatus;
            status.Commit = _project.Commit;
            return status;
        }

        [Authorize]
        [HttpGet("[action]")]
        public StatusResource Status()
        {
            var status = new StatusResource();
            status.Health = Health();
            status.Runtime = _runtime;
            status.App = _app;
            status.Dependencies = new List<DependencyResource>()
            {
                new DependencyResource
                {
                    AppName = "Identity Server Database",
                    Status = HealthContext.IdsDbStatus
                },
                new DependencyResource
                {
                    AppName = "Identity Manager Database",
                    Status = HealthContext.IdmDbStatus
                },
            };
            return status;
        }

        public class HealthResource
        {
            public string AppName { get; set; }
            public string Version { get; set; }
            public string Status { get; set; }
            public string Commit { get; set; }
        }

        public class StatusResource
        {
            public IEnumerable<DependencyResource> Dependencies { get; set; }
            public HealthResource Health { get; set; }
            public IRuntimeEnvironment Runtime { get; set; }
            public IApplicationEnvironment App { get; set; }
            public string MachineName { get { return Environment.MachineName; } }
        }

        public class DependencyResource
        {
            public string AppName { get; set; }
            public string Status { get; set; }
        }

        internal class ProjectInfo
        {
            public string Name;
            public string Commit;
            public string Version;
        }
    }
}