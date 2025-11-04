namespace Contracts.SupportAgentAssignMessages.SupportAgent
{
    public class SupportAgentUpdatedEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid TicketId { get; set; }
        public Guid AgentId { get; set; }
    }
}
