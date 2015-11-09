using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc.Razor;

namespace Fsw.Enterprise.AuthCentral
{
    internal class AreaViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            IEnumerable<string> user = viewLocations.Select(x => x.Replace("/Views/", "/Areas/UserAccount/Views/"));
            IEnumerable<string> admin = viewLocations.Select(x => x.Replace("/Views/", "/Areas/Admin/Views/"));

            return user.Concat(admin);
        }
    }
}