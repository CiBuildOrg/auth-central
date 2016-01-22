using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr.Events;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    /// <summary>
    /// An event handler for any event occuring from Admin user management tools.
    /// </summary>
    public class AdminEmailEventsHandler : EmailEventHandler<HierarchicalUserAccount>, IEventHandler<UserAccountCreatedByAdminEvent<HierarchicalUserAccount>>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="AdminEmailEventsHandler"/> with the given <see cref="IMessageFormatter{HierarchicalUserAccount}"/> and <see cref="IMessageDelivery"/> implementation.
        /// </summary>
        /// <param name="messageFormatter">The <see cref="IMessageFormatter{HierarchicalUserAccount}"/> implementation describing how emails should be built for this service.</param>
        /// <param name="messageDelivery">The <see cref="IMessageDelivery"/> implementation describing how emails should be sent from this service.</param>
        public AdminEmailEventsHandler(IMessageFormatter<HierarchicalUserAccount> messageFormatter, IMessageDelivery messageDelivery) : base(messageFormatter, messageDelivery)
        {
        }

        /// <summary>
        /// Processes <see cref="UserAccountCreatedByAdminEvent{HierarchicalUserAccount}"/> events.
        /// </summary>
        /// <param name="evt">Instance of the event to process.</param>
        public void Handle(UserAccountCreatedByAdminEvent<HierarchicalUserAccount> evt)
        {
            Process(evt, new { evt.VerificationKey });
        }
    }
}