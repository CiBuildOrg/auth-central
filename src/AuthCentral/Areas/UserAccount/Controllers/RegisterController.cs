﻿using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

using Microsoft.AspNet.Mvc;

using Fsw.Enterprise.AuthCentral.Models;
using System;
using Microsoft.AspNet.Authorization;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount
{
    [Authorize]
    [Area("UserAccount"), Route("[area]/[controller]")]
    public class RegisterController : Controller
    {
        readonly UserAccountService<HierarchicalUserAccount> _userAccountService;

        public RegisterController(UserAccountService<HierarchicalUserAccount> authSvc)
        {
            _userAccountService = authSvc;
        }

        public ActionResult Index()
        {
            return View(new RegisterInputModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = this._userAccountService.CreateAccount(model.Username, model.Password, model.Email);
                    ViewData["RequireAccountVerification"] = this._userAccountService.Configuration.RequireAccountVerification;
                    return View("Success", model);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public ActionResult Verify()
        {
            return View();
        }

        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public ActionResult Verify(string foo)
        {
            Guid userId;
            if(!Guid.TryParse(User.Claims.GetValue("sub"), out userId))
            {
                return new HttpUnauthorizedResult();
            }

            try
            {
                this._userAccountService.RequestAccountVerification(userId);
                return View("Success");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

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
