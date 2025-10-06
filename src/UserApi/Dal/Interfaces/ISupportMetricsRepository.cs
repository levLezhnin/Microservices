using CoreLib.Interfaces;
using UserApi.Dal.Models;

namespace UserApi.Dal.Interfaces
{
    public interface ISupportMetricsRepository : ICrudRepository<SupportMetricsDal, Guid>
    {
        Task<SupportMetricsDal> update(Guid id, int? ActiveTickets, int? TimesRated, double? rating);

        Task<SupportMetricsDal?> findBySupportIdAsync(Guid supportId);
        Task<SupportMetricsDal> findBySupportIdOrThrowAsync(Guid supportId);
        Task<Guid> findMostFreeSupportAgent();
    }
}
