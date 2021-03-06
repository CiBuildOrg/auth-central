﻿using System;
using System.Linq;

using Microsoft.AspNet.Mvc;

using Microsoft.AspNet.Authorization;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.Areas.Admin.Models;
using Fsw.Enterprise.AuthCentral.IdMgr;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Controllers
{
    [Authorize("FswAdmin")]
    [Area("Admin"), Route("[area]/[controller]")]
    public class UserProfileController : Controller
    {
        private UserAccountService<HierarchicalUserAccount> _userAccountService;

        // Even though this is an admin area, we don't need AdminUserAccountServiceContainer as long as this does not create accounts
        public UserProfileController(DefaultUserAccountServiceContainer container)
        {
            this._userAccountService = container.Service;
        }

        [HttpGet("[action]/{userId?}")]
        public IActionResult Edit(string userId, bool changed)
        {
            if (changed)
            {
                ViewBag.Message = "The requested change was processed successfully.";
            }

            Guid userGuid;
            if (!Guid.TryParse(userId, out userGuid))
            {
                return HttpBadRequest("Could not parse user Id.");
            }

            HierarchicalUserAccount user = _userAccountService.GetByID(userGuid);

            if (user != null)
            {
                bool canDeleteUsers = User.Claims.Any(claim => claim.Type == "fsw:testautomation" && claim.Value == "true");
                return View(new UserProfileModel
                {
                    Email = user.Email,
                    FamilyName = user.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value,
                    GivenName = user.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value,
                    Organization = user.Claims.FirstOrDefault(c => c.Type == "fsw:organization")?.Value,
                    Department = user.Claims.FirstOrDefault(c => c.Type == "fsw:department")?.Value,
                    IsLoginAllowed = user.IsLoginAllowed,
                    UserId = userId,
                    CanDeleteUsers = canDeleteUsers
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
                                                                    || claim.Type == "family_name"
                                                                    || claim.Type == "name"
                                                                    || claim.Type == "fsw:organization"
                                                                    || claim.Type == "fsw:department")));
                var claims = new UserClaimCollection();

                claims.Add("given_name", profile.GivenName);

                claims.Add("family_name", profile.FamilyName);

                claims.Add("name", string.Join(" ",
                    new string[] { profile.GivenName, profile.FamilyName }
                   .Where(name => !string.IsNullOrWhiteSpace(name))));

                if (!string.IsNullOrWhiteSpace(profile.Organization))
                {
                    claims.Add("fsw:organization", profile.Organization);
                }

                if(!string.IsNullOrWhiteSpace(profile.Department))
                {
                    claims.Add("fsw:department", profile.Department);
                }

                _userAccountService.AddClaims(userGuid, claims);
                return RedirectToAction("Edit", new { userId = profile.UserId, changed = true });
            }

            return View("Edit", profile);
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
            return RedirectToAction("Edit", new { userId = userId, changed = true });
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult Disable(string userId, int page, bool confirm)
        {
            Guid userGuid;
            if (!Guid.TryParse(userId, out userGuid))
            {
                return HttpBadRequest("Failed to parse userId.");
            }

            if(confirm)
            {
                _userAccountService.SetIsLoginAllowed(userGuid, false);
            }
            
            return RedirectToAction("Edit", new { userId = userId, changed = confirm });
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult Enable(string userId, int page)
        {
            Guid userGuid;
            if (!Guid.TryParse(userId, out userGuid))
            {
                return HttpBadRequest("Failed to parse userId.");
            }

            _userAccountService.SetIsLoginAllowed(userGuid, true);
            
            return RedirectToAction("Edit", new {  userId = userId, changed = true });

        }
    }
}
