using UserApi.Logic.Models;

namespace UserApi.Logic.Interfaces
{
    public interface ISupportMetricsService
    {
        Task<SupportMetricsLogic> insert(SupportMetricsLogic supportMetrics);

        Task<SupportMetricsLogic?> findByIdAsync(Guid id);
        Task<SupportMetricsLogic> findByIdOrThrowAsync(Guid id);

        Task<SupportMetricsLogic?> findBySupportIdAsync(Guid supportId);
        Task<SupportMetricsLogic> findBySupportIdOrThrowAsync(Guid supportId);
        
        Task<SupportMetricsLogic> addActiveTicket(Guid supportId);
        Task<SupportMetricsLogic> freeActiveTicket(Guid supportId);
        Task<SupportMetricsLogic> addRate(Guid supportId, int rate);
        Task<Guid> findMostFreeSupportAgent();

        Task<bool> deleteByIdAsync(Guid id);
    }
}
