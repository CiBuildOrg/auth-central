using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}