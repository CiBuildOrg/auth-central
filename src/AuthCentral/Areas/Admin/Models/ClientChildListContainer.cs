using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IdentityServer3.Core.Models;

namespace Fsw.Enterprise.AuthCentral.Areas.Admin.Models
{
    public class ClientChildListContainer<T>
    {
        public string ClientId { get; set; }
        public List<T> ChildList { get; set; }

    }
}
