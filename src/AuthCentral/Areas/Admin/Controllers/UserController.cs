using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

using BrockAllen.MembershipReboot.Hierarchical;
using BrockAllen.MembershipReboot;

using Fsw.Enterprise.AuthCentral.Crypto;
using Fsw.Enterprise.AuthCentral.Areas.Admin.Models;
using Fsw.Enterprise.AuthCentral.IdMgr;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Controllers
{
    [Authorize("FswAdmin")]
    [Area("Admin"), Route("[area]/[controller]")]
    public class UserController : Controller
    {
        EnvConfig _cfg;

        // Creates and updates accounts, finds single accounts by id or email
        UserAccountService<HierarchicalUserAccount> _userAccountService;

        // Used for querying multiple user accounts
        IBulkUserRepository<HierarchicalUserAccount> _repository;

        public UserController(EnvConfig cfg, 
            AdminUserAccountServiceContainer container,
            IBulkUserRepository<HierarchicalUserAccount> repository)
        {
            this._cfg = cfg;
            this._userAccountService = container.Service;
            this._repository = repository;
        }

        [HttpGet("{page?}")]
        public IActionResult Index(int page = 1, int pageSize = 25)
        {
            long count;
            IEnumerable<HierarchicalUserAccount> users = _repository.GetPagedUsers(page, pageSize, out count);
            UserListViewModel model = new UserListViewModel(users, page, pageSize, (int)count);
            model.CanDeleteUsers = User.Claims.Any(claim => claim.Type == "fsw:testautomation" && claim.Value == "true");
            return View("Index", model);
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
                    HierarchicalUserAccount account = _userAccountService.CreateAccount(model.Username, PasswordGenerator.GeneratePasswordOfLength(16), model.Email);
                    _userAccountService.SetConfirmedEmail(account.ID, account.Email);
                    _userAccountService.ResetPassword(account.ID);
                    AddClaims(account.ID, model);

                    return View("Success", model);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return Create();
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
        
        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public IActionResult Disable(string userId, int page)
        {
            Guid userGuid;
            if (!Guid.TryParse(userId, out userGuid))
            {
                return HttpBadRequest("Failed to parse userId.");
            }

            _userAccountService.SetIsLoginAllowed(userGuid, false);

            return RedirectToAction("Index", "User", new
            {
                page = page
            });
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

            return RedirectToAction("Index", "User", new
            {
                page = page
            });
        }
        
        [Authorize("FswAutomation")]
        [HttpPost("[action]/{userId}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string userId, int page)
        {
            Guid userGuid;
            if (!Guid.TryParse(userId, out userGuid))
            {
                return HttpBadRequest("Failed to parse userId.");
            }

            _userAccountService.DeleteAccount(userGuid);

            return RedirectToAction("Index", "User", new { page = page });

        }

        private void AddClaims(Guid accountId, CreateAccountInputModel model)
        {
            model.Email = model.Email.ToLowerInvariant().Trim();

            UserClaimCollection claims = new UserClaimCollection
            {
                new UserClaim("given_name", model.GivenName),
                new UserClaim("family_name", model.FamilyName),
                new UserClaim("name", string.Join(" ",
                    new string[] { model.GivenName, model.FamilyName }
                                   .Where(name => !string.IsNullOrWhiteSpace(name))
                ))
            };
            
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