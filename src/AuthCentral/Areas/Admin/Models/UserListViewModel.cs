using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    public class UserListViewModel
    {
        public IEnumerable<HierarchicalUserAccount> Accounts { get; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public bool CanDeleteUsers { get; set; }

        public UserListViewModel(IEnumerable<HierarchicalUserAccount> accounts, int pageNumber, int pageSize, int totalCount)
        {
            Accounts = accounts;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItemCount = totalCount;
        }
    }
}
