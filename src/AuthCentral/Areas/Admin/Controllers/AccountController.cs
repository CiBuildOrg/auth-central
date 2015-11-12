using System.ComponentModel.DataAnnotations;

using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

using BrockAllen.MembershipReboot.Hierarchical;
using BrockAllen.MembershipReboot;

using Fsw.Enterprise.AuthCentral.Models;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin
{
    [Area("Admin")]
    [Authorize("FswAdmin")]
    public class AccountController : Controller
    {
        EnvConfig _cfg;
        UserAccountService<HierarchicalUserAccount> _userAccountService;

        public AccountController(EnvConfig cfg, UserAccountService<HierarchicalUserAccount> userSvc)
        {
            this._cfg = cfg;
            this._userAccountService = userSvc;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(CreateAccountInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = this._userAccountService.CreateAccount(model.Username, model.Password, model.Email);
                    ViewData["RequireAccountVerification"] = this._userAccountService.Configuration.RequireAccountVerification;
                    return View("Success", model);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View("Index", model);
        }

    }
}