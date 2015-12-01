using System.ComponentModel.DataAnnotations;

using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

using BrockAllen.MembershipReboot.Hierarchical;
using BrockAllen.MembershipReboot;

using Fsw.Enterprise.AuthCentral.Crypto;
using Fsw.Enterprise.AuthCentral.Areas.Admin.Models;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin
{
    [Authorize("FswAdmin")]
    [Area("Admin"), Route("[area]/[controller]")]
    public class UserController : Controller
    {
        EnvConfig _cfg;
        UserAccountService<HierarchicalUserAccount> _userAccountService;

        public UserController(EnvConfig cfg, UserAccountService<HierarchicalUserAccount> userSvc)
        {
            this._cfg = cfg;
            this._userAccountService = userSvc;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreateAccountInputModel());
        }

        [HttpPost("[action]")]
        public ActionResult Create(CreateAccountInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string password = PasswordGenerator.GeneratePasswordOfLength(16);
                    HierarchicalUserAccount account = _userAccountService.CreateAccount(model.Username, password, model.Email);
                    _userAccountService.SetConfirmedEmail(account.ID, model.Email);
                    _userAccountService.ResetPassword(account.ID);

                    return View("Success", model);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View("Index", model);
        }

        public ActionResult Find(string email)
        {
            HierarchicalUserAccount account = _userAccountService.GetByEmail(email);
            if(account == null)
            {
                ViewBag.Message = "Failed to find an account with that e-mail address. Please try again.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Show", "UserClaim", new {
                userId = account.ID.ToString()
            });
        }
    }
}