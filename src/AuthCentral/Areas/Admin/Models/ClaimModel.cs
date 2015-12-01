using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using BrockAllen.MembershipReboot;
using System.ComponentModel.DataAnnotations;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{

    public class UserClaimModelContainer
    {
        public string UserId { get; set; }
        public IEnumerable<ClaimModel> UserClaims { get; set; }
    }

    public class ClientClaimModelContainer
    {
        public string ClientId { get; set; }
        public IEnumerable<ClaimModel> ClientClaims { get; set; }
    }

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

        [Required]
        public String Type { get; set; }
        [Required]
        public String Value { get; set; }

        public Claim ToClaim()
        {
            return new Claim(this.Type, this.Value);
        }

    }
}
