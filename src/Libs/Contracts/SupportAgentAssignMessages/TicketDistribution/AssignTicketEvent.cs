namespace Contracts.SupportAgentAssignMessages.TicketDistribution
{
    public record AssignTicketEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid TicketId { get; set; }
    }
}
