namespace Contracts.Messages
{
    public record TicketCreated
    {
        public Guid CorrelationId { get; init; }
        public Guid TicketId { get; init; }
        public Guid CreatorId { get; init; }
        public required string Title { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
