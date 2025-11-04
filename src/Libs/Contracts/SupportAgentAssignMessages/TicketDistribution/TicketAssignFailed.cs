namespace Contracts.SupportAgentAssignMessages.TicketDistribution
{
    public class TicketAssignFailed
    {
        public Guid CorrelationId { get; set; }
        public Guid TicketId { get; set; }
    }
}
