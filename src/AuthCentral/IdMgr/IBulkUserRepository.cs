using BrockAllen.MembershipReboot.Hierarchical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    public interface IBulkUserRepository<TAccount>
    {
        IEnumerable<TAccount> GetPagedUsers(int page, int pageSize, out long count);
    }
}
