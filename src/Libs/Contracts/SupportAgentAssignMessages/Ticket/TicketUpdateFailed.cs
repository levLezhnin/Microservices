namespace Contracts.SupportAgentAssignMessages.Ticket
{
    public record TicketUpdateFailed
    {
        public Guid CorrelationId { get; set; }
        public Guid TicketId { get; set; }
        public Guid AgentId { get; set; }
        public required string Reason { get; set; }
    }
}
