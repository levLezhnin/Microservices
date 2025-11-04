namespace Contracts.Messages
{
    public record StartTicketCreation
    {
        public Guid CorrelationId { get; set; }
        public Guid CreatorId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; } 
    }
}
