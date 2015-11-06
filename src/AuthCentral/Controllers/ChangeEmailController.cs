using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot;
using Fsw.Enterprise.AuthCentral.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Controllers
{
    [Authorize]
    public class ChangeEmailController : Controller
    {
        readonly UserAccountService _userAccountService;
        readonly AuthenticationService _authSvc;

        public ChangeEmailController(AuthenticationService authSvc)
        {
            _userAccountService = authSvc.UserAccountService;
            _authSvc = authSvc;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ChangeEmailRequestInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                _userAccountService.ChangeEmailRequest(User.GetUserID(), model.NewEmail);

                return _userAccountService.Configuration.RequireAccountVerification
                    ? View("ChangeRequestSuccess", model.NewEmail)
                    : View("Success");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Index", model);
        }

        [AllowAnonymous]
        public ActionResult Confirm(string id)
        {
            var account = _userAccountService.GetByVerificationKey(id);
            if (account == null)
            {
                ModelState.AddModelError("", BrockAllen.MembershipReboot.Resources.ValidationMessages.InvalidKey);
                return View("Index");
            }

            if (account.HasPassword())
            {
                var vm = new ChangeEmailFromKeyInputModel {Key = id};
                return View("Confirm", vm);
            }

            try
            {
                _userAccountService.VerifyEmailFromKey(id, out account);
                // since we've changed the email, we need to re-issue the cookie that
                // contains the claims.
                _authSvc.SignIn(account);
                return RedirectToAction("Success");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Confirm", null);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ChangeEmailFromKeyInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Confirm", model);
            }

            try
            {
                HierarchicalUserAccount account;
                _userAccountService.VerifyEmailFromKey(model.Key, model.Password, out account);
                    
                // since we've changed the email, we need to re-issue the cookie that
                // contains the claims.
                _authSvc.SignIn(account);
                return RedirectToAction("Success");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Confirm", model);
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}
