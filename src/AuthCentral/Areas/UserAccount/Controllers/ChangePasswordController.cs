using System.Linq;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.DataProtection;

using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

using Fsw.Enterprise.AuthCentral.IdMgr;
using Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models;
using Microsoft.AspNet.Authorization;
using System;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Controllers
{
    [Authorize]
    [Area("UserAccount"), Route("[area]/[controller]")]
    public class ChangePasswordController : Controller
    {
        readonly UserAccountService<HierarchicalUserAccount> userAccountService;

        public ChangePasswordController(MongoAuthenticationService authSvc)
        {
            this.userAccountService = authSvc.UserAccountService;
        }

 
        [HttpGet]
        public ActionResult Index()
        {

            Guid userId;
            if(!Guid.TryParse(User.Claims.GetValue("sub"), out userId))
            {
                return new HttpUnauthorizedResult();
            }

            var acct = this.userAccountService.GetByID(userId);
            if (acct.HasPassword())
            {
                return View(new ChangePasswordInputModel());
            }
            else
            {
                return View("SendPasswordReset");
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ChangePasswordInputModel model)
        {
            Guid userId;
            if(!Guid.TryParse(User.Claims.GetValue("sub"), out userId))
            {
                return new HttpUnauthorizedResult();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this.userAccountService.ChangePassword(userId, model.OldPassword, model.NewPassword);
                    return View("Success");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        [HttpPost("SendPasswordReset")]
        [ValidateAntiForgeryToken]
        public ActionResult SendPasswordReset()
        {
            Guid userId;
            if(!Guid.TryParse(User.Claims.GetValue("sub"), out userId))
            {
                return new HttpUnauthorizedResult();
            }

            try
            {
                var acct = this.userAccountService.GetByID(userId);
                this.userAccountService.ResetPassword(acct.Tenant, acct.Email);
                return View("Sent");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View("SendPasswordReset");
        }

    }
}
