using System.Security.Authentication;

using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

using Fsw.Enterprise.AuthCentral.IdMgr;
using Fsw.Enterprise.AuthCentral.Extensions;
using Fsw.Enterprise.AuthCentral.Areas.UserAccount.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
namespace Fsw.Enterprise.AuthCentral.Areas.UserAccount.Controllers
{
    [Authorize]
    [Area("UserAccount"), Route("[area]"), Route("account")]
    public class HomeController : Controller
    {
        readonly UserAccountService<HierarchicalUserAccount> _userAccountService;

        public HomeController(MongoAuthenticationService authSvc)
        {
            _userAccountService = authSvc.UserAccountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Details");
            }
            else
            {
                return View();
            }
        }

        [HttpGet("Details")]
        public IActionResult Details()
        {
            try
            {
                HierarchicalUserAccount user = _userAccountService.GetByID(User.GetId());
                UserAccountViewModel viewModel = new UserAccountViewModel(user);

                return View(viewModel);
            }
            catch(AuthenticationException)
            {
                return new HttpUnauthorizedResult();
            }
        }
    }
}
