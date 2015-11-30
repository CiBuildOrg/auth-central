using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models;
using Fsw.Enterprise.AuthCentral.IdMgr;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Controllers
{
    /// <summary>
    /// Controller for the user Password Reset flow.
    /// </summary>
    [AllowAnonymous]
    [Area("UserAccount"), Route("[area]/[controller]")]
    public class PasswordResetController : Controller
    {
        readonly UserAccountService<HierarchicalUserAccount> _userAccountService;
        readonly MongoAuthenticationService _authenticationService;

        /// <summary>
        /// Constructs a new <see cref="PasswordResetController"/> object.
        /// </summary>
        /// <param name="authenticationService">The authentication service used by the server.</param>
        public PasswordResetController(MongoAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _userAccountService = authenticationService.UserAccountService;
        }

        /// <summary>
        /// Main GET response for the Password Reset page.
        /// </summary>
        /// <returns>Index view.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action when the user posts from the index page.
        /// </summary>
        /// <param name="model">The values posted from the index page encapsulated in a <see cref="PasswordResetInputModel"/> object.</param>
        /// <returns>ResetSuccess view if post data is valid; otherwise Index view.</returns>
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


        /// <summary>
        /// Confirm action called as a GET method with a string <paramref name="id"/> parameter.
        /// </summary>
        /// <param name="id">The verification key associated with the password reset request.</param>
        /// <returns>Confirm view.</returns>
        [HttpGet("[action]/{id}")]
        public ActionResult Confirm(string id)
        {
            var vm = new ChangePasswordFromResetKeyInputModel()
            {
                Key = id
            };

            return View("Confirm", vm);
        }

        /// <summary>
        /// Confirm action called as a POST from the Confirm view.
        /// </summary>
        /// <param name="model">Contains the form fields from the Confirm view in a <see cref="ChangePasswordFromResetKeyInputModel"/> object.</param>
        /// <returns>Success view if password reset successfully completed; otherwise the Confirm view.</returns>
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

        /// <summary>
        /// Returns the Success view from a GET method call.
        /// </summary>
        /// <returns>Success view.</returns>
        [HttpGet("[action]")]
        public ActionResult Success()
        {
            return View();
        }
    }
}
