namespace Contracts.Messages
{
    public record TicketEventCreated
    {
        public Guid CorrelationId { get; init; }
        public Guid TicketId { get; init; }
        public string EventId { get; init; }
    }
}
