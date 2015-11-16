using System.ComponentModel.DataAnnotations;
using System.Linq;
using BrockAllen.MembershipReboot;
using Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models;
using Microsoft.AspNet.DataProtection;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Controllers
{
    [AllowAnonymous, Area("Admin")]
    public class PasswordResetController : Controller
    {
        UserAccountService userAccountService;
        AuthenticationService authenticationService;
        private readonly IDataProtector _protector;

        public PasswordResetController(AuthenticationService authenticationService, IDataProtector protector)
        {
            this.authenticationService = authenticationService;
            _protector = protector;
            this.userAccountService = authenticationService.UserAccountService;
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
                    var account = this.userAccountService.GetByEmail(model.Email);
                    if (account != null)
                    {
                        if (!account.PasswordResetSecrets.Any())
                        {
                            this.userAccountService.ResetPassword(model.Email);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetWithQuestions(PasswordResetWithSecretInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var answers =
                        model.Questions.Select(x => new PasswordResetQuestionAnswer { QuestionID = x.QuestionID, Answer = x.Answer });
                    this.userAccountService.ResetPasswordFromSecretQuestionAndAnswer(model.UnprotectedAccountID.Value, answers.ToArray());
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
                var account = this.userAccountService.GetByID(id.Value);
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

        public ActionResult Confirm(string id)
        {
            var vm = new ChangePasswordFromResetKeyInputModel()
            {
                Key = id
            };
            return View("Confirm", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ChangePasswordFromResetKeyInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BrockAllen.MembershipReboot.UserAccount account;
                    if (this.userAccountService.ChangePasswordFromResetKey(model.Key, model.Password, out account))
                    {
                        if (account.IsLoginAllowed && !account.IsAccountClosed)
                        {
                            this.authenticationService.SignIn(account);
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

        public ActionResult Success()
        {
            return View();
        }
    }
}
