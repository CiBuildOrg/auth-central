using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using BrockAllen.MembershipReboot;
using System.ComponentModel.DataAnnotations;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    /// <summary>
    /// A container for user claims.
    /// </summary>
    public class UserClaimModelContainer
    {
        /// <summary>
        /// User's ID in string form.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Claims for this user.
        /// </summary>
        public IEnumerable<ClaimModel> UserClaims { get; set; }
    }

    /// <summary>
    /// A container for client claims.
    /// </summary>
    public class ClientClaimModelContainer
    {
        /// <summary>
        /// Client's ID in string form.
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Claims for this client.
        /// </summary>
        public IEnumerable<ClaimModel> ClientClaims { get; set; }
    }

    /// <summary>
    /// An annotated model for creating and updating claims.
    /// </summary>
    public class ClaimModel
    {
        public ClaimModel() { }
        public ClaimModel(string type, string value)
        {
            this.Type = type;
            this.Value = value;
        }

        public ClaimModel(Claim claim) : this(claim.Type, claim.Value) { }
        public ClaimModel(UserClaim claim) : this(claim.Type, claim.Value) { }

        /// <summary>
        /// The type of claim.
        /// </summary>
        [Required]
        public String Type { get; set; }
        
        /// <summary>
        /// Value of the claim.
        /// </summary>
        [Required]
        public String Value { get; set; }

        public Claim ToClaim()
        {
            return new Claim(this.Type, this.Value);
        }
    }
}
