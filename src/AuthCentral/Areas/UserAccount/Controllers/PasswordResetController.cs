using System.ComponentModel.DataAnnotations;
using System.Linq;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models;
using Fsw.Enterprise.AuthCentral.IdMgr;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.DataProtection;
using Microsoft.AspNet.Authorization;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Controllers
{
    [AllowAnonymous]
    [Area("UserAccount"), Route("[area]/[controller]")]
    public class PasswordResetController : Controller
    {
        readonly UserAccountService<HierarchicalUserAccount> _userAccountService;
        readonly MongoAuthenticationService _authenticationService;
        private readonly IDataProtector _protector;

        public PasswordResetController(MongoAuthenticationService authenticationService, IDataProtectionProvider provider)
        {
            this._authenticationService = authenticationService;
            _protector = provider.CreateProtector(GetType().FullName);
            this._userAccountService = authenticationService.UserAccountService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PasswordResetInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = this._userAccountService.GetByEmail(model.Email);
                    if (account != null)
                    {
                        if (!account.PasswordResetSecrets.Any())
                        {
                            this._userAccountService.ResetPassword(model.Email);
                            return View("ResetSuccess");
                        }

                        var vm = new PasswordResetWithSecretInputModel(_protector, account.ID);
                        vm.Questions =
                            account.PasswordResetSecrets.Select(
                                x => new PasswordResetSecretViewModel
                                {
                                    QuestionID = x.PasswordResetSecretID,
                                    Question = x.Question
                                }).ToArray();

                        return View("ResetWithQuestions", vm);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid email");
                    }
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View("Index");
        }


        [HttpPost("[action]")]
        [ValidateAntiForgeryToken]
        public ActionResult ResetWithQuestions(PasswordResetWithSecretInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var answers =
                        model.Questions.Select(x => new PasswordResetQuestionAnswer { QuestionID = x.QuestionID, Answer = x.Answer });
                    this._userAccountService.ResetPasswordFromSecretQuestionAndAnswer(model.UnprotectedAccountID.Value, answers.ToArray());
                    return View("ResetSuccess");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var id = model.UnprotectedAccountID;
            if (id != null)
            {
                var account = this._userAccountService.GetByID(id.Value);
                if (account != null)
                {
                    var vm = new PasswordResetWithSecretInputModel(_protector, account.ID);
                    vm.Questions =
                        account.PasswordResetSecrets.Select(
                            x => new PasswordResetSecretViewModel
                            {
                                QuestionID = x.PasswordResetSecretID,
                                Question = x.Question
                            }).ToArray();
                    return View("ResetWithQuestions", vm);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet("[action]/{id}")]
        public ActionResult Confirm(string id)
        {
            var vm = new ChangePasswordFromResetKeyInputModel()
            {
                Key = id
            };
            return View("Confirm", vm);
        }

        [HttpPost("[action]/{id?}")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ChangePasswordFromResetKeyInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HierarchicalUserAccount account;
                    if (this._userAccountService.ChangePasswordFromResetKey(model.Key, model.Password, out account))
                    {
                        if (account.IsLoginAllowed && !account.IsAccountClosed)
                        {
                            this._authenticationService.SignIn(account);
                            if (account.RequiresTwoFactorAuthCodeToSignIn())
                            {
                                return RedirectToAction("TwoFactorAuthCodeLogin", "Login");
                            }
                            if (account.RequiresTwoFactorCertificateToSignIn())
                            {
                                return RedirectToAction("CertificateLogin", "Login");
                            }
                        }

                        return RedirectToAction("Success");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error changing password. The key might be invalid.");
                    }
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View();
        }

        [HttpGet("[action]")]
        public ActionResult Success()
        {
            return View();
        }
    }
}
