using System;
using System.Linq;
﻿using System.Security.Authentication;
using System.ComponentModel.DataAnnotations;

using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

using Fsw.Enterprise.AuthCentral.Extensions;
using Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models;

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
                    model.Email = model.Email.ToLowerInvariant().Trim();

                    var account = this._userAccountService.CreateAccount(model.Username, model.Password, model.Email);
                    ViewData["RequireAccountVerification"] = this._userAccountService.Configuration.RequireAccountVerification;
                    UserClaimCollection claims = new UserClaimCollection
                    {
                        new UserClaim("given_name", model.GivenName),
                        new UserClaim("family_name", model.FamilyName),
                        new UserClaim("name", string.Join(" ",
                            new string[] { model.GivenName, model.FamilyName }
                                           .Where(name => !string.IsNullOrWhiteSpace(name))
                        ))
                    };

                    if (model.Email.EndsWith("@foodservicewarehouse.com") || model.Email.EndsWith("@fsw.com"))
                    {
                        claims.Add(new UserClaim("fsw:organization", "FSW"));
                    }
                    else
                    {
                        string emailDomain = model.Email.Split('@')[1];
                        claims.Add(new UserClaim("fsw:organization", emailDomain));
                    }

                    _userAccountService.AddClaims(account.ID, claims);
                    
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
            try
            {
                this._userAccountService.RequestAccountVerification(User.GetId());
                return View("Success");
            }
            catch (AuthenticationException)
            {
                return new HttpUnauthorizedResult();
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
