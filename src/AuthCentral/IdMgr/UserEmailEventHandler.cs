using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Microsoft.Extensions.Logging;

namespace Fsw.Enterprise.AuthCentral.IdMgr.Notifications.Email.EventHandlers
{
    public class UserEmailEventHandler :
        EmailEventHandler<HierarchicalUserAccount>,
        IEventHandler<EmailVerifiedEvent<HierarchicalUserAccount>>,
        IEventHandler<AccountCreatedEvent<HierarchicalUserAccount>>
    {
        private ILogger _logger;
        
        public UserEmailEventHandler(ILoggerFactory loggerFactory, IMessageFormatter<HierarchicalUserAccount> messageFormatter)
            : base(messageFormatter)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().ToString());
        }

        public UserEmailEventHandler(ILoggerFactory loggerFactory, IMessageFormatter<HierarchicalUserAccount> messageFormatter, IMessageDelivery messageDelivery)
            : base(messageFormatter, messageDelivery)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().ToString());
        }
        
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