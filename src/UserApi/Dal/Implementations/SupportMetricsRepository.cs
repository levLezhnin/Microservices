using CoreLib.Exceptions;
using CoreLib.Repository;
using Microsoft.EntityFrameworkCore;
using UserApi.Dal.Interfaces;
using UserApi.Dal.Models;

namespace UserApi.Dal.Implementations
{
    public class SupportMetricsRepository : CrudRepository<SupportMetricsDal, Guid>, ISupportMetricsRepository
    {
        public SupportMetricsRepository(DbContext dbContext) : base(dbContext)
        { }

        public async Task<SupportMetricsDal> update(Guid id, int? activeTickets, int? timesRated, double? rating)
        {
            SupportMetricsDal dal = await findByIdOrThrowAsync(id);
            if (activeTickets != null)
            {
                dal.ActiveTickets = activeTickets.Value;
            }
            if (timesRated != null)
            {
                dal.TimesRated = timesRated.Value;
            }
            if (rating != null)
            {
                dal.Rating = rating.Value;
            }
            return await update(dal);
        }

        public async Task<SupportMetricsDal?> findBySupportIdAsync(Guid supportId)
        {
            return await _dbSet.FirstOrDefaultAsync(supportMetrics => supportMetrics.SupportId.Equals(supportId));
        }

        public async Task<SupportMetricsDal?> findBySupportIdOrThrowAsync(Guid supportId)
        {
            return await _dbSet.FirstOrDefaultAsync(supportMetrics => supportMetrics.SupportId.Equals(supportId)) ??
                throw new EntityNotFoundException($"Метрики агента с id: {supportId} не найдены!");
        }

        public async Task<Guid> findMostFreeSupportAgent()
        {
            return await _dbSet.OrderBy(
                supportMetrics => supportMetrics.ActiveTickets
            )
            .Select(supportMetrics => supportMetrics.SupportId)
            .FirstOrDefaultAsync();
        }
    }
}
