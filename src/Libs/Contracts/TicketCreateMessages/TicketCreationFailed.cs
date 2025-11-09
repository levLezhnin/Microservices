namespace Contracts.Messages
{
    public record TicketCreationFailed
    {
        public Guid CorrelationId { get; init; }
        public string Reason { get; init; } = string.Empty;
    }
}
