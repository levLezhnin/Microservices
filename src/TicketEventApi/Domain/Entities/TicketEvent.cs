using CoreLib.Common;
using MongoDB.Bson;

namespace Domain.Entities
{
    public record TicketEvent : BaseEntityDal<ObjectId>
    {
        public required EventType eventType { get; set; }
        public required Guid ticketId { get; set; }
        public required string description { get; set; }
        public required DateTime createdAt { get; set; }
    }
}
