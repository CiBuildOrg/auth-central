using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fsw.Enterprise.AuthCentral.IdMgr.Notifications.Email
{
    public class MultipartMessageBody
    {
        public string PlainText { get; set; }
        public string Html { get; set; }
    }
}
