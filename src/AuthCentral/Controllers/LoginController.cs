using IdentityServer3.Core.ViewModels;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        [HttpPost]
        public ActionResult Index(LoginCredentials model)
        {
            return View();
        }
    }
}