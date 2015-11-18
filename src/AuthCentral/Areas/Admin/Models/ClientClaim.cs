using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IdentityServer3.Core.Models;
using System.Security.Claims;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    public class ClientClaimContainer
    {
        public string ClientId { get; set; }
        public List<ClientClaim> ClientClaims { get; set; }

    }

    public class ClientClaim
    {
        public ClientClaim() { }
        public ClientClaim(string type, string value)
        {
            this.Type = type;
            this.Value = value;
        }
        public ClientClaim(Claim claim):this(claim.Type, claim.Value) { }

        public String Type { get; set; }
        public String Value { get; set; }

        public Claim ToClaim()
        {
            return new Claim(this.Type, this.Value);
        }

    }
}
