namespace Contracts.SupportAgentAssignMessages.SupportAgent
{
    public class SupportAgentUpdateEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid TicketId { get; set; }
        public Guid AgentId { get; set; }
    }
}
