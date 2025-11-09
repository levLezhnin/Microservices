namespace Contracts.SupportAgentAssignMessages.TicketDistribution
{
    public record SupportAgentChosen
    {
        public Guid CorrelationId { get; set; }
        public Guid TicketId { get; set; }
        public Guid AgentId { get; set; }
    }
}
