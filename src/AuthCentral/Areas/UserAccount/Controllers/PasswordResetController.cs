using System.ComponentModel.DataAnnotations;
using System.Linq;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models;
using Fsw.Enterprise.AuthCentral.IdMgr;
using Microsoft.AspNet.DataProtection;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Controllers
{
    [AllowAnonymous]
    [Area("UserAccount"), Route("[area]/[controller]")]
    public class PasswordResetController : Controller
    {
        readonly UserAccountService<HierarchicalUserAccount> _userAccountService;
        readonly MongoAuthenticationService _authenticationService;
        private readonly IDataProtector _protector;

        public PasswordResetController(MongoAuthenticationService authenticationService, IDataProtectionProvider provider)
        {
            this._authenticationService = authenticationService;
            _protector = provider.CreateProtector(GetType().FullName);
            this._userAccountService = authenticationService.UserAccountService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PasswordResetInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = this._userAccountService.GetByEmail(model.Email);
                    if (account != null)
                    {
                        if (!account.PasswordResetSecrets.Any())
                        {
                            this._userAccountService.ResetPassword(model.Email);
                            return View("ResetSuccess");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid email");
                    }
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
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
            if (ModelState.IsValid)
            {
                try
                {
                    HierarchicalUserAccount account;
                    if (this._userAccountService.ChangePasswordFromResetKey(model.Key, model.Password, out account))
                    {
                        if (account.IsLoginAllowed && !account.IsAccountClosed)
                        {
                            this._authenticationService.SignIn(account);
                            if (account.RequiresTwoFactorAuthCodeToSignIn())
                            {
                                return RedirectToAction("TwoFactorAuthCodeLogin", "Login");
                            }
                            if (account.RequiresTwoFactorCertificateToSignIn())
                            {
                                return RedirectToAction("CertificateLogin", "Login");
                            }
                        }

                        return RedirectToAction("Success");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error changing password. The key might be invalid.");
                    }
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
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
