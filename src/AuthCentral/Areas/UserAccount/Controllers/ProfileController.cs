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
                return View(new UserProfileModel
                {
                    Email = user.Email,
                    FamilyName = user.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value,
                    GivenName = user.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value,
                    Organization = user.Claims.FirstOrDefault(c => c.Type == "fsw:organization")?.Value,
                    Department = user.Claims.FirstOrDefault(c => c.Type == "fsw:department")?.Value,
                    UserId = user.ID.ToString()
                });
            }
            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("[action]")]
        public IActionResult ChangeName(UserProfileModel profile)
        {
            Guid userGuid;
            if (!Guid.TryParse(profile.UserId, out userGuid))
            {
                return HttpBadRequest("Failed to parse userId.");
            }

            if (ModelState.IsValid)
            {
                var user = _userAccountService.GetByID(userGuid);

                _userAccountService.RemoveClaims(userGuid,
                    new UserClaimCollection(user.Claims.Where(claim => claim.Type == "given_name"
                                                                    || claim.Type == "family_name"
                                                                    || claim.Type == "name")));
                var claims = new UserClaimCollection();

                claims.Add("given_name", profile.GivenName);

                claims.Add("family_name", profile.FamilyName);

                claims.Add("name", string.Join(" ",
                    new string[] { profile.GivenName, profile.FamilyName }
                   .Where(name => !string.IsNullOrWhiteSpace(name))));

                _userAccountService.AddClaims(userGuid, claims);
                return RedirectToAction("Edit", new { userId = profile.UserId, changed = true });
            }

            return View("Edit", profile);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("[action]")]
        public IActionResult ChangePassword(UserProfileModel profile)
        {
            Guid userGuid;
            if (!Guid.TryParse(profile.UserId, out userGuid))
            {
                return HttpBadRequest("Failed to parse userId.");
            }

            if(profile.NewPasswordConfirm != profile.NewPassword)
            {
                return HttpBadRequest("New passwords must match.");
            }

            _userAccountService.ChangePassword(userGuid, profile.OldPassword, profile.NewPassword);
            _authSvc.SignIn(_userAccountService.GetByID(userGuid));
            return RedirectToAction("Edit", new { userId = profile.UserId, changed = true });
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("[action]")]
        public IActionResult ChangeEmail(UserProfileModel profile)
        {
            Guid userGuid;
            if (!Guid.TryParse(profile.UserId, out userGuid))
            {
                return HttpBadRequest("Failed to parse userId.");
            }
            
            _userAccountService.ChangeEmailRequest(userGuid, profile.Email);

            return RedirectToAction("Edit", new { userId = profile.UserId, changed = true });
        }
    }
}
