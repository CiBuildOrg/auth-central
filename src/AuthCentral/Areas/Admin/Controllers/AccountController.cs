using Microsoft.AspNet.Mvc;
using Fsw.Enterprise.AuthCentral.Models;
using BrockAllen.MembershipReboot.Hierarchical;
using BrockAllen.MembershipReboot;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin
{
    [Area("Admin")]
    [Authorize]
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
                    // TODO: Make this error a little smarter.
                    // associating it with email for now, since it'll contain
                    // (among other things) an 'Email already in use' error
                    ModelState.AddModelError("Email", ex.Message);
                }
            }

            return View("Index", model);
        }

    }
}