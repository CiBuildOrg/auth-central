using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot;
using Fsw.Enterprise.AuthCentral.Models;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Controllers
{
    [AllowAnonymous, Route("[controller]")]
    public class RegisterController : Controller
    {
        UserAccountService userAccountService;
        AuthenticationService authSvc;

        public RegisterController(AuthenticationService authSvc)
        {
            this.authSvc = authSvc;
            this.userAccountService = authSvc.UserAccountService;
        }

        public ActionResult Index()
        {
            return View(new RegisterInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = this.userAccountService.CreateAccount(model.Username, model.Password, model.Email);
                    ViewData["RequireAccountVerification"] = this.userAccountService.Configuration.RequireAccountVerification;
                    return View("Success", model);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        [Route("Verify")]
        public ActionResult Verify()
        {
            return View();
        }

        [HttpPost, Route("Verify")]
        [ValidateAntiForgeryToken]
        public ActionResult Verify(string foo)
        {
            try
            {
                this.userAccountService.RequestAccountVerification(User.GetUserID());
                return View("Success");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        [Route("Cancel")]
        public ActionResult Cancel(string id)
        {
            try
            {
                bool closed;
                this.userAccountService.CancelVerification(id, out closed);
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
