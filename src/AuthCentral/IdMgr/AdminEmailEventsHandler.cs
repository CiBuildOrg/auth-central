using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

namespace Fsw.Enterprise.AuthCentral.IdMgr.Notifications.Email.EventHandlers
{
    public class DefaultEmailEventHandler :
        EmailEventHandler<HierarchicalUserAccount>,
        IEventHandler<PasswordChangedEvent<HierarchicalUserAccount>>,
        IEventHandler<PasswordResetRequestedEvent<HierarchicalUserAccount>>,
        IEventHandler<PasswordResetSecretAddedEvent<HierarchicalUserAccount>>,
        IEventHandler<PasswordResetSecretRemovedEvent<HierarchicalUserAccount>>,
        IEventHandler<UsernameReminderRequestedEvent<HierarchicalUserAccount>>,
        IEventHandler<AccountClosedEvent<HierarchicalUserAccount>>,
        IEventHandler<AccountReopenedEvent<HierarchicalUserAccount>>,
        IEventHandler<UsernameChangedEvent<HierarchicalUserAccount>>,
        IEventHandler<EmailChangeRequestedEvent<HierarchicalUserAccount>>,
        IEventHandler<EmailChangedEvent<HierarchicalUserAccount>>,
        IEventHandler<MobilePhoneChangedEvent<HierarchicalUserAccount>>,
        IEventHandler<MobilePhoneRemovedEvent<HierarchicalUserAccount>>,
        IEventHandler<CertificateAddedEvent<HierarchicalUserAccount>>,
        IEventHandler<CertificateRemovedEvent<HierarchicalUserAccount>>,
        IEventHandler<LinkedAccountAddedEvent<HierarchicalUserAccount>>,
        IEventHandler<LinkedAccountRemovedEvent<HierarchicalUserAccount>>
    {
        public DefaultEmailEventHandler(IMessageFormatter<HierarchicalUserAccount> messageFormatter)
            : base(messageFormatter)
        { }

        public DefaultEmailEventHandler(IMessageFormatter<HierarchicalUserAccount> messageFormatter, IMessageDelivery messageDelivery)
            : base(messageFormatter, messageDelivery)
        { }

        public void Handle(PasswordResetRequestedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt, new { evt.VerificationKey });
        }

        public void Handle(PasswordChangedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt);
        }

        public void Handle(PasswordResetSecretAddedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt);
        }

        public void Handle(PasswordResetSecretRemovedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt);
        }

        public void Handle(UsernameReminderRequestedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt);
        }

        public void Handle(AccountClosedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt);
        }

        public void Handle(AccountReopenedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt, new { evt.VerificationKey });
        }

        public void Handle(UsernameChangedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt);
        }

        public void Handle(EmailChangeRequestedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt, new { evt.OldEmail, evt.NewEmail, evt.VerificationKey });
        }

        public void Handle(EmailChangedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt, new { evt.OldEmail, evt.VerificationKey });
        }

        public void Handle(MobilePhoneChangedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt);
        }

        public void Handle(MobilePhoneRemovedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt);
        }

        public void Handle(CertificateAddedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt, new { evt.Certificate.Thumbprint, evt.Certificate.Subject });
        }

        public void Handle(CertificateRemovedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt, new { evt.Certificate.Thumbprint, evt.Certificate.Subject });
        }

        public void Handle(LinkedAccountAddedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt, new { evt.LinkedAccount.ProviderName });
        }

        public void Handle(LinkedAccountRemovedEvent<HierarchicalUserAccount> evt)
        {
            Process(evt, new { evt.LinkedAccount.ProviderName });
        }
    }
}