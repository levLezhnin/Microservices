namespace UserConnectionLib.ConnectionServices.DtoModels
{
    internal class SupportMetricsResponseDto
    {
        public required Guid SupportId { get; init; }
        public required int ActiveTickets { get; init; }
        public required int TimesRated { get; init; }
        public required double Rating { get; init; }
    }
}
