using CoreLib.Common;
using CoreLib.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("tickets")]
    public record Ticket : BaseEntityDal<Guid>
    {
        public required Guid creatorId { get; init; }
        public Guid? assignedSupportAgentId { get; set; }

        [Column(TypeName = "varchar(255)")]
        public required string title { get; set; }
        [Column(TypeName = "varchar(1023)")]
        public string? description { get; set; }
        
        [Column(TypeName = "varchar(50)")]
        public required TicketStatuses status { get; set; }

        [DataType(DataType.DateTime)]
        public required DateTime createdAt { get; init; }
        [DataType(DataType.DateTime)]
        public DateTime? updatedAt { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? resolvedAt { get; set; }
    }
}
