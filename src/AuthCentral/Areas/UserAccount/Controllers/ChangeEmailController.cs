using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr;
using Fsw.Enterprise.AuthCentral.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount
{
    [Authorize]
    [Area("UserAccount"), Route("[area]/[controller]")]
    public class ChangeEmailController : Controller
    {
        readonly UserAccountService<HierarchicalUserAccount> _userAccountService;
        readonly MongoAuthenticationService _authSvc;

        public ChangeEmailController(MongoAuthenticationService authSvc)
        {
            _authSvc = authSvc;
            _userAccountService = authSvc.UserAccountService;
        }

        public IActionResult Index()
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
        [HttpGet("[action]/{id}")]
        public ActionResult Confirm(string id)
        {
            HierarchicalUserAccount account = _userAccountService.GetByVerificationKey(id);

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
        [HttpPost("[action]")]
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

        [AllowAnonymous]
        [HttpGet("[action]")]
        public ActionResult Success()
        {
            return View();
        }
    }
}
