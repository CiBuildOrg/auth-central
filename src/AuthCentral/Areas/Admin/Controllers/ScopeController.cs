using Fsw.Enterprise.AuthCentral.Areas.Admin.Models;
using Fsw.Enterprise.AuthCentral.MongoStore.Admin;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Controllers
{
    [Area("Admin"), Route("[area]/[controller]")]
    public class ScopeController : Controller
    {
        private readonly IScopeService _scopeService;

        public ScopeController(IScopeService scopeService)
        {
            _scopeService = scopeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new ScopeListModel(_scopeService.Get().Result);
            return View(model);
        }

        [HttpPost("[action]")]
        public IActionResult Edit()
        {
            return RedirectToAction("Index");
        }
    }
}