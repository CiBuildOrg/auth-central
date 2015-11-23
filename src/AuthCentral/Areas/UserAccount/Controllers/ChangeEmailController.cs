using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models;
using Fsw.Enterprise.AuthCentral.IdMgr;
using Fsw.Enterprise.AuthCentral.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Controllers
{
    /// <summary>
    /// Controller that handles user change-email and email verification requests.
    /// </summary>
    [Authorize]
    [Area("UserAccount"), Route("[area]/[controller]")]
    public class ChangeEmailController : Controller
    {
        readonly UserAccountService<HierarchicalUserAccount> _userAccountService;
        readonly MongoAuthenticationService _authSvc;

        /// <summary>
        /// Instantiates a new copy of <see cref="ChangeEmailController"/>.
        /// </summary>
        /// <param name="authSvc">Active instance of the authentication service against which to verify emails and accounts.</param>
        public ChangeEmailController(MongoAuthenticationService authSvc)
        {
            _authSvc = authSvc;
            _userAccountService = authSvc.UserAccountService;
        }

        /// <summary>
        /// Initial page.  Requires authentication. Allows a user to enter a new email address to replace their current.
        /// </summary>
        /// <returns>The Index view.</returns>
        public IActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// Action handling the POST method from the Index view.
        /// Starts the email verification flow.
        /// </summary>
        /// <param name="model">
        /// Form data from the Index view.</param>
        /// <returns>Success view if email change was successful and no verification required;
        /// ChangeRequestSuccess view if email change was successful but verification required;
        /// otherwise Index view.
        /// </returns>
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
                _userAccountService.ChangeEmailRequest(ClaimsPrincipalExtensions.GetUserID(User), model.NewEmail);

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

        /// <summary>
        /// Handles Confirm GET call.
        /// Starts confirmation of the email verification.
        /// </summary>
        /// <param name="id">Verification key sent by the email verification request.</param>
        /// <returns>Confirm view </returns>
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

        /// <summary>
        /// Handles POST action from the Confirm view.
        /// Attempts to verify the user's email and password.
        /// </summary>
        /// <param name="model">Form data from the Confirm view.</param>
        /// <returns>Success view if email and password verification was successful; otherwise the Confirm view.</returns>
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

        /// <summary>
        /// Handles Success GET call.
        /// </summary>
        /// <returns>Success view.</returns>
        [AllowAnonymous]
        [HttpGet("[action]")]
        public ActionResult Success()
        {
            return View();
        }
    }
}
