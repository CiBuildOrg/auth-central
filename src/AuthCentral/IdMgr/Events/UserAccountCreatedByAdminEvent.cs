using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.IdMgr.Events
{
    public class UserAccountCreatedByAdminEvent<TAccount> : UserAccountEvent<TAccount> where TAccount : UserAccount {
        public string VerificationKey { get; set; }
        public string InitialPassword { get; set; }
    }
}
