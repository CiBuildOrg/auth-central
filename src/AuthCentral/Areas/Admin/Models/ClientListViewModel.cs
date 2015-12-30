using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using IdentityServer3.Core.Models;

using Fsw.Enterprise.AuthCentral.MongoStore;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    public class ClientListViewModel
    {
        public IEnumerable<Client> Clients { get; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }

        public ClientListViewModel(ClientPagingResult clientPagingResult, int pageNumber, int pageSize)
        {
            Clients = clientPagingResult.Collection;
            PageNumber = pageNumber;
            PageSize = pageSize;

            if(clientPagingResult.HasMore)
            {
                TotalItemCount = pageNumber * pageSize + 1;
            }
            else
            {
                TotalItemCount = pageNumber * pageSize;
            }


        }
    }
}
