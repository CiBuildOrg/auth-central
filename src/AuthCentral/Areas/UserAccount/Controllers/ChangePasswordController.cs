using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.DataProtection;

using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

using Fsw.Enterprise.AuthCentral.IdMgr;
using Fsw.Enterprise.AuthCentral.Extensions;
using Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models;
using System.Security.Authentication;

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

            try
            {
                Guid userId = User.GetId();

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
            catch(AuthenticationException)
            {
                return new HttpUnauthorizedResult();
            }
       }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ChangePasswordInputModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    this.userAccountService.ChangePassword(User.GetId(), model.OldPassword, model.NewPassword);
                    return View("Success");
                }
                catch(AuthenticationException)
                {
                    return new HttpUnauthorizedResult();
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
            try
            {
                var acct = this.userAccountService.GetByID(User.GetId());
                this.userAccountService.ResetPassword(acct.Tenant, acct.Email);
                return View("Sent");
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

    }
}
