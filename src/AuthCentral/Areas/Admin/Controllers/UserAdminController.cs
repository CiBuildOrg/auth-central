using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Controllers
{
    public abstract class UserAdminController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values.ContainsKey("userId"))
            {
                ViewBag.ClientId = context.RouteData.Values["userId"];
            }
            base.OnActionExecuting(context);
        }
    }
}
