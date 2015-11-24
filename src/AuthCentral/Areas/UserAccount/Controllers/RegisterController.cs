using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models;
using System;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Controllers
{
    /// <summary>
    /// Controller that handles user self-registration actions and flow.
    /// </summary>
    [Authorize]
    [Area("UserAccount"), Route("[area]/[controller]")]
    public class RegisterController : Controller
    {
        readonly UserAccountService<HierarchicalUserAccount> _userAccountService;

        /// <summary>
        /// Instantiates a new instance of the <see cref="RegisterController"/>
        /// </summary>
        /// <param name="authSvc">Active instance of the user account service against which new users can be registered.</param>
        public RegisterController(UserAccountService<HierarchicalUserAccount> authSvc)
        {
            _userAccountService = authSvc;
        }

        /// <summary>
        /// Initial view. GET method.
        /// </summary>
        /// <returns>Index view</returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(new RegisterInputModel());
        }

        /// <summary>
        /// Action handling POST method from the Index view.
        /// Attempts to create the new user.
        /// </summary>
        /// <param name="model">Form data from the Index view in a <see cref="RegisterInputModel"/> object.</param>
        /// <returns>Success view if the user was created; otherwise the Index view.</returns>
        [HttpPost]
        [AllowAnonymous]
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

        /// <summary>
        /// GET response for the Verify action.
        /// </summary>
        /// <returns>Verify view.</returns>
        [HttpGet("[action]")]
        [AllowAnonymous]
        public ActionResult Verify()
        {
            return View();
        }

        /// <summary>
        /// POST response for the Verify action.  
        /// </summary>
        /// <param name="foo">String parameter for the POST method to be unique.</param>
        /// <returns>Success view if the account was verified; otherwise the Verify view.</returns>
        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public ActionResult Verify(string foo)
        {
            Guid userId;
            if(!Guid.TryParse(User.Claims.GetValue("sub"), out userId))
            {
                return new HttpUnauthorizedResult();
            }

            try
            {
                this._userAccountService.RequestAccountVerification(userId);
                return View("Success");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        /// <summary>
        /// Handles the Cancel GET call.  Cancels the registration of the account with the verification key set to <paramref name="id"/>
        /// </summary>
        /// <param name="id">Verification key sent as part of email verification.</param>
        /// <returns>Closed view if cancellation was successful; otherwise Cancel view.</returns>
        [HttpGet("[action]/{id}")]
        [AllowAnonymous]
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
