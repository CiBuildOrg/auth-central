/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Fsw.Enterprise.AuthCentral.IdMgr.Events
{
    public class AuthCentralEmailEventsHandler<T> :
        EmailAccountEventsHandler<T>,
        IEventHandler<UserAccountCreatedByAdminEvent<T>>
        where T : UserAccount
    {
        public AuthCentralEmailEventsHandler(IMessageFormatter<T> messageFormatter)
            : base(messageFormatter)
        {
        }

        public AuthCentralEmailEventsHandler(IMessageFormatter<T> messageFormatter, IMessageDelivery messageDelivery)
            : base(messageFormatter, messageDelivery)
        {
        }

        public void Handle(UserAccountCreatedByAdminEvent<T> evt)
        {
            Process(evt, new { evt.VerificationKey });
        }
    }

    public class EmailAccountEventsHandler : EmailAccountEventsHandler<UserAccount>
    {
        public EmailAccountEventsHandler(IMessageFormatter<UserAccount> messageFormatter)
            : base(messageFormatter)
        {
        }
        public EmailAccountEventsHandler(IMessageFormatter<UserAccount> messageFormatter, IMessageDelivery messageDelivery)
            : base(messageFormatter, messageDelivery)
        {
        }
    }
}
