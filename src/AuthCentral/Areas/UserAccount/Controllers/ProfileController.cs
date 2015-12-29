using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models;
using Fsw.Enterprise.AuthCentral.Extensions;
using System.Security.Authentication;
using System.ComponentModel.DataAnnotations;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Controllers
{
    [Authorize]
    [Area("UserAccount"), Route("[area]/[controller]")]
    public class ProfileController : Controller
    {
        private UserAccountService<HierarchicalUserAccount> _userAccountService;
        private AuthenticationService<HierarchicalUserAccount> _authSvc;

        public ProfileController(UserAccountService<HierarchicalUserAccount> accountSvc, AuthenticationService<HierarchicalUserAccount> authSvc)
        {
            this._userAccountService = accountSvc;
            this._authSvc = authSvc;
        }

        // GET: /<controller>/
        [Authorize]
        [HttpGet]
        public IActionResult Edit(bool changed)
        {
            if (changed)
            {
                ViewBag.Message = "The requested change was processed successfully.";
            }
            
            HierarchicalUserAccount user = _userAccountService.GetByID(User.GetId());

            if (user != null)
            {
                return View("Edit", new UserProfileModel
                {
                    Name = new UserNameModel
                    {
                        FamilyName = user.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value,
                        GivenName = user.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value,
                    },
                    Email = user.Email,
                    Organization = user.Claims.FirstOrDefault(c => c.Type == "fsw:organization")?.Value,
                    Department = user.Claims.FirstOrDefault(c => c.Type == "fsw:department")?.Value
                });
            }
            return View("Edit");
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("[action]")]
        public IActionResult ChangeName(UserNameModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userAccountService.GetByID(User.GetId());

                _userAccountService.RemoveClaims(user.ID,
                    new UserClaimCollection(user.Claims.Where(claim => claim.Type == "given_name"
                                                                    || claim.Type == "family_name"
                                                                    || claim.Type == "name")));
                var claims = new UserClaimCollection();

                claims.Add("given_name", model.GivenName);

                claims.Add("family_name", model.FamilyName);

                claims.Add("name", string.Join(" ",
                    new string[] { model.GivenName, model.FamilyName }
                   .Where(name => !string.IsNullOrWhiteSpace(name))));

                _userAccountService.AddClaims(user.ID, claims);
                return RedirectToAction("Edit", new { changed = true });
            }

            return Edit(false);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("[action]")]
        public IActionResult ChangePassword(ChangePasswordInputModel profile)
        {
            try
            {
                var acct = _userAccountService.GetByID(User.GetId());
                _userAccountService.ResetPassword(acct.Tenant, acct.Email);
                return View("PasswordSent");
            }
            catch (AuthenticationException)
            {
                return new HttpUnauthorizedResult();
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View("SendPasswordReset");
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("[action]")]
        public IActionResult ChangeEmail(ChangeEmailRequestInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                _userAccountService.ChangeEmailRequest(User.GetId(), model.NewEmail);

                if (_userAccountService.Configuration.RequireAccountVerification)
                {
                    return View("EmailConfirmationSent", model);
                }
                else
                {
                    return RedirectToAction("Success", "ChangeEmail", null);
                }
            }
            catch (AuthenticationException)
            {
                return new HttpUnauthorizedResult();
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Index", model);
        }
    }
}
