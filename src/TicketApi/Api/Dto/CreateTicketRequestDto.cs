namespace Api.Dto
{
    public record CreateTicketRequestDto
    {
        public required Guid creatorId { get; set; }
        public required string title { get; set; }
        public string? description { get; set; }
    }
}
