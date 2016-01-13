using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using IdentityServer3.Core.Models;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    public class ScopeListModel
    {
        public ScopeListModel(IEnumerable<Scope> scopes)
        {
            Scopes = scopes.Select(s => new ScopeModel(s));
        }

        public IEnumerable<ScopeModel> Scopes { get; private set; }
    }

    public class ScopeModel
    {
        internal readonly Scope IdsScope;

        public ScopeModel()
        {
            IdsScope = new Scope();
        }

        public ScopeModel(Scope scope)
        {
            IdsScope = scope;
        }

        public bool Enabled
        {
            get { return IdsScope.Enabled; }
            set { IdsScope.Enabled = value; }
        }

        public string Name
        {
            get { return IdsScope.Name; }
            set { IdsScope.Name = value; }
        }

        [Display(Name = "Display Name")]
        public string DisplayName
        {
            get { return IdsScope.DisplayName; }
            set { IdsScope.DisplayName = value; }
        }

        public string Description
        {
            get { return IdsScope.Description; }
            set { IdsScope.Description = value; }
        }

        public bool Required
        {
            get { return IdsScope.Required; }
            set { IdsScope.Required = value; }
        }

        public bool Emphasize
        {
            get { return IdsScope.Emphasize; }
            set { IdsScope.Emphasize = value; }
        }

        public ScopeType Type
        {
            get { return IdsScope.Type; }
            set { IdsScope.Type = value; }
        }

        [ReadOnly(true)]
        public IEnumerable<ScopeClaim> Claims => IdsScope.Claims;

        [Display(Name = "Include all claims for users")]
        public bool IncludeAllClaimsForUser
        {
            get { return IdsScope.IncludeAllClaimsForUser; }
            set { IdsScope.IncludeAllClaimsForUser = value; }
        }

        [Display(Name = "Claims Rule")]
        public string ClaimsRule
        {
            get { return IdsScope.ClaimsRule; }
            set { IdsScope.ClaimsRule = value; }
        }

        [Display(Name = "Show in Discovery Doc")]
        public bool ShowInDiscoveryDocument
        {
            get { return IdsScope.ShowInDiscoveryDocument; }
            set { IdsScope.ShowInDiscoveryDocument = value; }
        }
    }
}