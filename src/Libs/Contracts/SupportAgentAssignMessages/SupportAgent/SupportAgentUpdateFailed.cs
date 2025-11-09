namespace Contracts.SupportAgentAssignMessages.SupportAgent
{
    public record SupportAgentUpdateFailed
    {
        public Guid CorrelationId { get; set; }
        public Guid TicketId { get; set; }
        public Guid AgentId { get; set; }
        public required string Reason { get; set; }
    }
}
