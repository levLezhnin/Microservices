namespace Contracts.SupportAgentAssignMessages.Ticket
{
    public record TicketUpdateEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid TicketId { get; set; }
        public Guid AgentId { get; set; }
    }
}
