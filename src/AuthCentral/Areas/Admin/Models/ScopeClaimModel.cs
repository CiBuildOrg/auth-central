using System.ComponentModel;
using IdentityServer3.Core.Models;
using Microsoft.AspNet.Mvc;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    /// <summary>
    /// MVC Model for ScopeClaims.
    /// </summary>
    public class ScopeClaimModel
    {
        /// <summary>
        /// Creates a new instance of <see cref="ScopeClaimModel"/> from an existing scope claim.
        /// </summary>
        /// <param name="scopeName">Name / ID of the scope that owns the given <paramref name="claim"/></param>
        /// <param name="claim">The <see cref="ScopeClaim"/> this model represents.</param>
        public ScopeClaimModel(string scopeName, ScopeClaim claim)
        {
            ScopeName = scopeName;
            ClaimId = claim.Name;
            ScopeClaim = claim;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ScopeClaimModel"/> for a new claim.
        /// </summary>
        public ScopeClaimModel()
        {
            ScopeClaim = new ScopeClaim();
        }

        /// <summary>
        /// Name / ID of the scope that has this claim.
        /// </summary>
        [ReadOnly(true), HiddenInput]
        public string ScopeName { get; set; }

        /// <summary>
        /// The original ID of this claim, in case the name changes.
        /// </summary>
        [ReadOnly(true), HiddenInput]
        public string ClaimId { get; set; }

        internal ScopeClaim ScopeClaim { get; }

        /// <summary>
        /// Name of the claim so that it can be used in javascript.
        /// </summary>
        public string SafeName => Name.Replace(':', '_');

        /// <summary>
        /// Name of the claim.
        /// </summary>
        public string Name
        {
            get { return ScopeClaim.Name; }
            set { ScopeClaim.Name = value; }
        }

        /// <summary>
        /// Description of the claim;
        /// </summary>
        public string Description
        {
            get { return ScopeClaim.Description; }
            set { ScopeClaim.Description = value; }
        }

        /// <summary>
        /// Whether the claim should always be included in the token, requested or not.
        /// </summary>
        public bool AlwaysIncludeInIdToken
        {
            get { return ScopeClaim.AlwaysIncludeInIdToken; }
            set { ScopeClaim.AlwaysIncludeInIdToken = value; }
        }
    }
}