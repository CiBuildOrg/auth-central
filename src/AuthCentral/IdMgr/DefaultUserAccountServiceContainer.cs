using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    public class DefaultUserAccountServiceContainer
    {
        public UserAccountService<HierarchicalUserAccount> Service { get; set; }
    }
}
