using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// This was a failed attempt to extend the EventBusUserAccountRepository class
// its purpose is to add the ability to create an account without raising events.

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    public class AuthCentralUserAccountRepository<TAccount> : EventBusUserAccountRepository<TAccount> where TAccount : UserAccount
    {
        public AuthCentralUserAccountRepository(IEventSource source, IUserAccountRepository<TAccount> inner, IEventBus validationBus, IEventBus eventBus) : base(source, inner, validationBus, eventBus) { }

        // This could be either an overload (like this) or a separate method that does not trigger events.
        // The overload is slightly more illustrative of what we would like to achieve.
        public void Add(TAccount account, bool sendEmail = false)
        {
            if(sendEmail)
            {
                base.Add(account);
            }
            else
            {
                // this is what we'd like to do here, but this method and field are both private.
                // RaiseValidation();
                // inner.Add(Account);
            }
        }
    }
}
