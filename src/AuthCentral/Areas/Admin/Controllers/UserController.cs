using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

using BrockAllen.MembershipReboot.Hierarchical;
using BrockAllen.MembershipReboot;

using Fsw.Enterprise.AuthCentral.Crypto;
using Fsw.Enterprise.AuthCentral.Areas.Admin.Models;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Controllers
{
    [Authorize("FswAdmin")]
    [Area("Admin"), Route("[area]/[controller]")]
    public class UserController : Controller
    {
        EnvConfig _cfg;
        UserAccountService<HierarchicalUserAccount> _userAccountService;

        public UserController(EnvConfig cfg, UserAccountService<HierarchicalUserAccount> userSvc)
        {
            this._cfg = cfg;
            this._userAccountService = userSvc;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("[action]")]
        public IActionResult Create()
        {
            return View(new CreateAccountInputModel());
        }

        [HttpPost("[action]")]
        public IActionResult Create(CreateAccountInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string password = PasswordGenerator.GeneratePasswordOfLength(16);
                    HierarchicalUserAccount account = _userAccountService.CreateAccount(model.Username, password, model.Email);
                    _userAccountService.SetConfirmedEmail(account.ID, model.Email);
                    _userAccountService.ResetPassword(account.ID);
                    AddClaims(account.ID, model);

                    return View("Success", model);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View("Index", model);
        }

        [HttpPost("[action]")]
        public IActionResult Find(string email)
        {
            HierarchicalUserAccount account = _userAccountService.GetByEmail(email);
            if(account == null)
            {
                ViewBag.Message = "Failed to find an account with that e-mail address. Please try again.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Show", "UserClaim", new {
                userId = account.ID.ToString()
            });
        }

        private void AddClaims(Guid accountId, CreateAccountInputModel model)
        {
            model.Email = model.Email.ToLowerInvariant().Trim();

            UserClaimCollection claims = new UserClaimCollection
            {
                new UserClaim("given_name", model.GivenName),
                new UserClaim("family_name", model.FamilyName),
                new UserClaim("name", string.Join(" ",
                    new string[] { model.GivenName, model.MiddleName, model.FamilyName }
                                   .Where(name => !string.IsNullOrWhiteSpace(name))
                ))
            };

            if (!string.IsNullOrWhiteSpace(model.MiddleName))
            {
                claims.Add(new UserClaim("middle_name", model.MiddleName));
            }

            if (model.IsAuthCentralAdmin) {
                claims.Add(new UserClaim("fsw:authcentral:admin", "true"));
            }
            
            if(!string.IsNullOrWhiteSpace(model.Organization))
            {
                claims.Add(new UserClaim("fsw:organization", model.Organization));
            }
            else if(model.Email.EndsWith("@foodservicewarehouse.com") || model.Email.EndsWith("@fsw.com"))
            {
                claims.Add(new UserClaim("fsw:organization", "FSW"));
            }
            else
            {
                string emailDomain = model.Email.Split('@')[1];
                claims.Add(new UserClaim("fsw:organization", emailDomain));
            }

            if(!string.IsNullOrWhiteSpace(model.Department))
            {
                claims.Add(new UserClaim("fsw:department", model.Department));
            }
            
            _userAccountService.AddClaims(accountId, claims);
        }
    }
}