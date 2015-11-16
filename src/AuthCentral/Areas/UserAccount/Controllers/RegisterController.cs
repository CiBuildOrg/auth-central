using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

using Microsoft.AspNet.Mvc;

using Fsw.Enterprise.AuthCentral.Models;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount
{
    [AllowAnonymous]
    [Area("UserAccount")]
    public class RegisterController : Controller
    {
        readonly UserAccountService<HierarchicalUserAccount> _userAccountService;

        public RegisterController(UserAccountService<HierarchicalUserAccount> authSvc)
        {
            _userAccountService = authSvc;
        }

        public ActionResult Index()
        {
            return View(new RegisterInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RegisterInputModel model)
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
            return View(model);
        }

        [Route("Verify")]
        public ActionResult Verify()
        {
            return View();
        }

        [HttpPost, Route("Verify")]
        [ValidateAntiForgeryToken]
        public ActionResult Verify(string foo)
        {
            try
            {
                this._userAccountService.RequestAccountVerification(User.GetUserID());
                return View("Success");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        [Route("Cancel")]
        public ActionResult Cancel(string id)
        {
            try
            {
                bool closed;
                this._userAccountService.CancelVerification(id, out closed);
                if (closed)
                {
                    return View("Closed");
                }
                else
                {
                    return View("Cancel");
                }
            }
            catch(ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View("Error");
        }
    }
}
