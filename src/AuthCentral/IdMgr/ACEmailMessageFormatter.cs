using System;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    internal class ACEmailMessageFormatter : EmailMessageFormatter<HierarchicalUserAccount>
    {
        public ACEmailMessageFormatter(AuthCentralAppInfo appInfo) : base(appInfo)
        {
        }

        public ACEmailMessageFormatter(Lazy<ApplicationInformation> appInfo) : base(appInfo)
        {
        }


    }
}