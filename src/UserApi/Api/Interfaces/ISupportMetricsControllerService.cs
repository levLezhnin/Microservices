using UserApi.Api.Dto.Response;

namespace UserApi.Api.Interfaces
{
    public interface ISupportMetricsControllerService
    {
        Task<SupportMetricsResponseDto> findBySupportIdOrThrowAsync(Guid supportId);
        Task<Guid> findMostFreeSupportAgent();

        Task<SupportMetricsResponseDto> addActiveTicket(Guid supportId);
        Task<SupportMetricsResponseDto> freeActiveTicket(Guid supportId);
        Task<SupportMetricsResponseDto> addRate(Guid supportId, int rating);
    }
}
