namespace Contracts.SupportAgentAssignMessages.Init
{
    public record StartTicketAssign
    {
        public Guid CorrelationId { get; set; }
        public Guid TicketId { get; set; }
    }
}
