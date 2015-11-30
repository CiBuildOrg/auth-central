using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    public class UserClaimContainer
    {
        public string UserId { get; set; }
        public IEnumerable<UserClaim> UserClaims { get; set; }
    }
}
