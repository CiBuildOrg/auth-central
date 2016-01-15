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
        private readonly Scope _scope;

        public ScopeModel()
        {
            _scope = new Scope();
        }

        public ScopeModel(Scope scope)
        {
            _scope = scope;
        }

        public bool Enabled
        {
            get { return _scope.Enabled; }
            set { _scope.Enabled = value; }
        }

        public string Name
        {
            get { return _scope.Name; }
            set { _scope.Name = value; }
        }

        [Display(Name = "Display Name")]
        public string DisplayName
        {
            get { return _scope.DisplayName; }
            set { _scope.DisplayName = value; }
        }

        public string Description
        {
            get { return _scope.Description; }
            set { _scope.Description = value; }
        }

        public bool Required
        {
            get { return _scope.Required; }
            set { _scope.Required = value; }
        }

        public bool Emphasize
        {
            get { return _scope.Emphasize; }
            set { _scope.Emphasize = value; }
        }

        public ScopeType Type
        {
            get { return _scope.Type; }
            set { _scope.Type = value; }
        }

        public List<ScopeClaim> Claims
        {
            get { return _scope.Claims; }
            set { _scope.Claims = value; }
        }

        [Display(Name = "Include all claims for users")]
        public bool IncludeAllClaimsForUser
        {
            get { return _scope.IncludeAllClaimsForUser; }
            set { _scope.IncludeAllClaimsForUser = value; }
        }

        [Display(Name = "Claims Rule")]
        public string ClaimsRule
        {
            get { return _scope.ClaimsRule; }
            set { _scope.ClaimsRule = value; }
        }

        [Display(Name = "Show in Discovery Doc")]
        public bool ShowInDiscoveryDocument
        {
            get { return _scope.ShowInDiscoveryDocument; }
            set { _scope.ShowInDiscoveryDocument = value; }
        }
    }
}