using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

namespace Fsw.Enterprise.AuthCentral.IdMgr.Notifications.Email.EventHandlers
{
    public class UserEmailEventHandler :
        EmailEventHandler<HierarchicalUserAccount>,
        IEventHandler<EmailVerifiedEvent<HierarchicalUserAccount>>,
        IEventHandler<AccountCreatedEvent<HierarchicalUserAccount>>
    {
        public UserEmailEventHandler(IMessageFormatter<HierarchicalUserAccount> messageFormatter)
            : base(messageFormatter)
        { }

        public UserEmailEventHandler(IMessageFormatter<HierarchicalUserAccount> messageFormatter, IMessageDelivery messageDelivery)
            : base(messageFormatter, messageDelivery)
        { }
        
        public void Handle(AccountCreatedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt, new { evt.InitialPassword, evt.VerificationKey });
        }

        public void Handle(EmailVerifiedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt);
        }
    }
}