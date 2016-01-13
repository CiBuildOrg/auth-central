using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    public class ScopeListModel
    {
        public ScopeListModel(IEnumerable<Scope> scopes)
        {
            Scopes = scopes;
        }

        public IEnumerable<Scope> Scopes { get; private set; }
    }
}