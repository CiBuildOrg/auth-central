using Microsoft.AspNet.Mvc;
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
    public class UserClaimController : Controller
    {
        EnvConfig _cfg;
        UserAccountService<HierarchicalUserAccount> _userAccountService;

        public UserClaimController(EnvConfig cfg, UserAccountService<HierarchicalUserAccount> userSvc)
        {
            this._cfg = cfg;
            this._userAccountService = userSvc;
        }

        [HttpGet("[action]/{userId}")]
        public ActionResult Show(string userId, bool changed)
        {
            if(changed)
            {
                ViewBag.Message = "The requested change was processed successfully.";
            }

            Guid userGuid;
            if(!Guid.TryParse(userId, out userGuid))
            {
                return HttpBadRequest("Failed to parse user Id.");
            }

            // Not awaitable!
            HierarchicalUserAccount user = _userAccountService.GetByID(userGuid);

            if (user == null)
            {
                ViewBag.Message = string.Format("The Auth Central User with UserId {0} could not be found.", userId);
                return HttpBadRequest("Failed to find user with the given Id.");
            }

            var model = new UserClaimModelContainer()
            {
                UserId = user.ID.ToString(),
                UserClaims = user.Claims.Where(claim => claim.Type != "name"
                                                     && claim.Type != "given_name"
                                                     && claim.Type != "family_name"
                                                     && claim.Type != "fsw:organization"
                                                     && claim.Type != "fsw:department")
                                        .Select(claim => new ClaimModel(claim))
            };

            return View(model);
        }

        [HttpGet("[action]/{userId}")]
        public ActionResult Create(string userId)
        {
            Guid userGuid;
            if (!Guid.TryParse(userId, out userGuid))
            {
                return HttpBadRequest("Failed to parse userId.");
            }
            
            HierarchicalUserAccount user = _userAccountService.GetByID(userGuid);

            if (user == null)
            {
                ViewBag.Message = string.Format("The Auth Central User with UserId {0} could not be found.", userId);
                return RedirectToAction("Index");
            }

            var model = new UserClaimModelContainer()
            {
                UserId = user.ID.ToString(),
                UserClaims = new List<ClaimModel>(new[] { new ClaimModel() })
            };

            return View("Create", model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("[action]/{userId}")]
        public ActionResult Delete(string userId, ClaimModel userClaim)
        {
            Guid userGuid;
            if (!Guid.TryParse(userId, out userGuid))
            {
                return HttpBadRequest("Failed to parse user Id.");
            }

            _userAccountService.RemoveClaim(userGuid, userClaim.Type, userClaim.Value);

            return RedirectToAction("Show", new { userId = userId, changed = true });
        }

        [ValidateAntiForgeryToken]
        [HttpPost("[action]")]
        public ActionResult Save(UserClaimModelContainer cmc)
        {
            Guid userGuid;
            if (!Guid.TryParse(cmc.UserId, out userGuid))
            {
                return HttpBadRequest("Failed to parse user Id.");
            }

            if (ModelState.IsValid)
            {
                _userAccountService.AddClaims(userGuid, new UserClaimCollection(cmc.UserClaims.Select(c => new Claim(c.Type, c.Value))));

                return RedirectToAction("Show", new { userId = cmc.UserId, changed = true });
            }

            return Create(cmc.UserId);
        }
    }
}