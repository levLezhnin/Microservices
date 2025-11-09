namespace Contracts.Messages
{
    public record TicketEventCreationFailed
    {
        public Guid CorrelationId { get; init; }
        public Guid TicketId { get; init; }
        public string Reason { get; init; } = string.Empty;
    }
}
