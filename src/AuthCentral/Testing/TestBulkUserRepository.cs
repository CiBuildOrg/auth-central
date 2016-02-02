using System;
using System.Collections.Generic;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr;

namespace Fsw.Enterprise.AuthCentral.Testing
{
    public class TestBulkUserRepository : IBulkUserRepository<HierarchicalUserAccount>
    {
        private static Dictionary<Guid, HierarchicalUserAccount> _accounts;

        private Dictionary<Guid, HierarchicalUserAccount> Accounts
            => _accounts ?? (_accounts = new Dictionary<Guid, HierarchicalUserAccount>());

        public TestBulkUserRepository()
        {
            if (!Accounts.ContainsKey(TestUser.PreloadUser.ID))
                Accounts.Add(TestUser.PreloadUser.ID, TestUser.PreloadUser);

            if (!Accounts.ContainsKey(TestUser.TestAdmin.ID))
                Accounts.Add(TestUser.TestAdmin.ID, TestUser.TestAdmin);
        }

        public IEnumerable<HierarchicalUserAccount> GetPagedUsers(int page, int pageSize, out long count)
        {
            count = Accounts.Count;

            return Accounts.Values;
        }
    }
}