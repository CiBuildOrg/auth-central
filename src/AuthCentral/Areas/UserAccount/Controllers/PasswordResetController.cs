using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models;
using Fsw.Enterprise.AuthCentral.IdMgr;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Controllers
{
    [AllowAnonymous]
    [Area("UserAccount"), Route("[area]/[controller]")]
    public class PasswordResetController : Controller
    {
        readonly UserAccountService<HierarchicalUserAccount> _userAccountService;
        readonly MongoAuthenticationService _authenticationService;

        {
            _authenticationService = authenticationService;
            _userAccountService = authenticationService.UserAccountService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PasswordResetInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                HierarchicalUserAccount account = _userAccountService.GetByEmail(model.Email);

                if (account != null)
                {
                    _userAccountService.ResetPassword(model.Email);
                    return View("ResetSuccess");
                }

                ModelState.AddModelError("", "Invalid email");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Index");
        }

        [HttpGet("[action]/{id}")]
        public ActionResult Confirm(string id)
        {
            var vm = new ChangePasswordFromResetKeyInputModel()
            {
                Key = id
            };

            return View("Confirm", vm);
        }

        [HttpPost("[action]/{id?}")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ChangePasswordFromResetKeyInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                HierarchicalUserAccount account;
                if (_userAccountService.ChangePasswordFromResetKey(model.Key, model.Password, out account))
                {
                    if (!account.IsLoginAllowed || account.IsAccountClosed)
                    {
                        return RedirectToAction("Success");
                    }

                    _authenticationService.SignIn(account);
                        
                    // TODO: Uncomment if we do two-factor or certificate auth.
                    //if (account.RequiresTwoFactorAuthCodeToSignIn())
                    //{
                    //    return RedirectToAction("TwoFactorAuthCodeLogin", "Login");
                    //}

                    //if (account.RequiresTwoFactorCertificateToSignIn())
                    //{
                    //    return RedirectToAction("CertificateLogin", "Login");
                    //}

                    return RedirectToAction("Success");
                }

                ModelState.AddModelError("", "Error changing password. The key might be invalid.");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        [HttpGet("[action]")]
        public ActionResult Success()
        {
            return View();
        }
    }
}
