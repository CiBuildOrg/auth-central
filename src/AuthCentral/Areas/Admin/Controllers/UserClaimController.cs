﻿using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

using BrockAllen.MembershipReboot.Hierarchical;
using BrockAllen.MembershipReboot;

using System;
using System.Collections.Generic;
using System.Linq;
using Fsw.Enterprise.AuthCentral.Areas.Admin.Models;
using System.Security.Claims;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Controllers
{
    [Authorize("FswAdmin")]
    [Area("Admin"), Route("[area]/[controller]")]
    public class UserClaimController : UserAdminController
    {
        EnvConfig _cfg;
        UserAccountService<HierarchicalUserAccount> _userAccountService;

        public UserClaimController(EnvConfig cfg, UserAccountService<HierarchicalUserAccount> userSvc)
        {
            this._cfg = cfg;
            this._userAccountService = userSvc;
        }

        [HttpGet("[action]/{userId}")]
        public ActionResult Show(string userId)
        {
            Guid userGuid;
            if(!Guid.TryParse(userId, out userGuid))
            {
                return HttpUnauthorized();
            }

            // Not awaitable!
            HierarchicalUserAccount user = _userAccountService.GetByID(userGuid);

            if (user == null)
            {
                ViewBag.Message = string.Format("The Auth Central User with UserId {0} could not be found.", userId);
                return HttpUnauthorized();
            }

            var model = new ClaimModelContainer()
            {
                ClaimantId = user.ID.ToString(),
                Claims = user.Claims.Select(claim => new ClaimModel(claim))
            };

            return View(model);
        }

        [HttpGet("[action]/{userId}")]
        public ActionResult Create(string userId)
        {
            Guid userGuid;
            if (!Guid.TryParse(userId, out userGuid))
            {
                return HttpUnauthorized();
            }
            
            HierarchicalUserAccount user = _userAccountService.GetByID(userGuid);

            if (user == null)
            {
                ViewBag.Message = string.Format("The Auth Central User with UserId {0} could not be found.", userId);
                return RedirectToAction("Index");
            }

            var model = new ClaimModelContainer()
            {
                ClaimantId = user.ID.ToString(),
                Claims = new List<ClaimModel>(new[] { new ClaimModel() })
            };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("[action]/{userId}")]
        public ActionResult Delete(string userId, ClaimModel userClaim)
        {
            Guid userGuid;
            if (!Guid.TryParse(userId, out userGuid))
            {
                return HttpUnauthorized();
            }

            _userAccountService.RemoveClaim(userGuid, userClaim.Type, userClaim.Value);

            return RedirectToAction("Show", new { userId = userId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost("[action]")]
        public ActionResult Save(ClaimModelContainer cmc)
        {
            Guid userGuid;
            if (!Guid.TryParse(cmc.ClaimantId, out userGuid))
            {
                return HttpUnauthorized();
            }

            if (ModelState.IsValid)
            {
                _userAccountService.AddClaims(userGuid, new UserClaimCollection(cmc.Claims.Select(c => new Claim(c.Type, c.Value))));

                return RedirectToAction("Show", new { userId = cmc.ClaimantId });
            }
            
            HierarchicalUserAccount user = _userAccountService.GetByID(userGuid);

            if (user == null)
            {
                ViewBag.Message = string.Format("The Auth Central User with UserId {0} could not be found.", cmc.ClaimantId);
                return RedirectToAction("Index");
            }

            var model = new ClaimModelContainer()
            {
                ClaimantId = user.ID.ToString(),
                Claims = new List<ClaimModel>(new[] { new ClaimModel() })
            };

            return View("Create", model);
        }
    }
}