namespace UserApi.Logic.Models
{
    public class SupportMetricsLogic
    {
        public required Guid Id { get; init; }
        public required Guid SupportId { get; set; }
        public required int ActiveTickets { get; set; }
        public required int TimesRated { get; set; }
        public required double Rating { get; set; }
    }
}
