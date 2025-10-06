using CoreLib.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.Dal.Models
{
    [Table("support_metrics")]
    public record SupportMetricsDal : BaseEntityDal<Guid>
    {
        public required Guid SupportId { get; init; }
        [ForeignKey("SupportId")]
        public UserDal SupportAgent { get; init; }

        public required int ActiveTickets { get; set; }
        public required int TimesRated { get; set; }
        public required double Rating { get; set; }
    }
}
