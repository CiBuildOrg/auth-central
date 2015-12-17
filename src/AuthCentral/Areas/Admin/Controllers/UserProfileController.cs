using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Microsoft.AspNet.Mvc;

using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

using Fsw.Enterprise.AuthCentral.MongoStore.Admin;
using Microsoft.AspNet.Authorization;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.Areas.Admin.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Controllers
{
    [Authorize("FswAdmin")]
    [Area("Admin"), Route("[area]/[controller]")]
    public class UserProfileController : Controller
    {
        private UserAccountService<HierarchicalUserAccount> _userAccountService;

        public UserProfileController(UserAccountService<HierarchicalUserAccount> svc)
        {
            this._userAccountService = svc;
        }

        [HttpGet("[action]/{userId?}")]
        public IActionResult Edit(string userId)
        {
            Guid userGuid;
            if (!Guid.TryParse(userId, out userGuid))
            {
                return HttpBadRequest("Failed to parse userId.");
            }

            HierarchicalUserAccount user = _userAccountService.GetByID(userGuid);

            if (user != null)
            {
                return View(new UserProfileModel
                {
                    Email = user.Email,
                    FamilyName = user.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value,
                    MiddleName = user.Claims.FirstOrDefault(c => c.Type == "middle_name")?.Value,
                    GivenName = user.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value,
                    Organization = user.Claims.FirstOrDefault(c => c.Type == "fsw:organization")?.Value,
                    Department = user.Claims.FirstOrDefault(c => c.Type == "fsw:department")?.Value,
                    UserId = userId
                });
            }
            else
            {
                ViewBag.Message = string.Format("The Auth Central User with UserId {0} could not be found.", userId);
                return View();
            }
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult Save(UserProfileModel profile)
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
                                                                    || claim.Type == "middle_name"
                                                                    || claim.Type == "family_name"
                                                                    || claim.Type == "name"
                                                                    || claim.Type == "fsw:organization"
                                                                    || claim.Type == "fsw:department")));
                var claims = new UserClaimCollection();
                claims.Add("given_name", profile.GivenName);
                if(profile.MiddleName != null)
                {
                    claims.Add("middle_name", profile.MiddleName);
                }
                claims.Add("family_name", profile.FamilyName);
                claims.Add("name", string.Join(" ",
                    new string[] { profile.GivenName, profile.MiddleName, profile.FamilyName }
                   .Where(name => !string.IsNullOrWhiteSpace(name))));

                claims.Add("fsw:organization", profile.Organization);
                claims.Add("fsw:department", profile.Department);

                _userAccountService.AddClaims(userGuid, claims);
                
                // ViewBag.Message = string.Format("The Auth Central User {0} was successfully saved!", user.Email);
                return RedirectToAction("Edit", new { userId = profile.UserId });
            }

            return View("Edit");
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeEmail(string userId, string email)
        {
            Guid userGuid;
            if (!Guid.TryParse(userId, out userGuid))
            {
                return HttpBadRequest("Failed to parse userId.");
            }
            _userAccountService.ChangeEmailRequest(userGuid, email);
            // ViewBag.Message = "The change e-mail request was submitted successfully.";
            return RedirectToAction("Edit", new { userId = userId });
        }
    }
}
