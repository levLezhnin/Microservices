using CoreLib.Interfaces;
using UserApi.Dal.Interfaces;
using UserApi.Dal.Models;
using UserApi.Logic.Interfaces;
using UserApi.Logic.Models;

namespace UserApi.Logic.Implementations
{
    internal class SupportMetricsService : ISupportMetricsService
    {

        private readonly ISupportMetricsRepository _repository;
        private readonly ITwoWayMapper<SupportMetricsDal, SupportMetricsLogic> _mapper;

        public SupportMetricsService(ISupportMetricsRepository repository, ITwoWayMapper<SupportMetricsDal, SupportMetricsLogic> mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SupportMetricsLogic> insert(SupportMetricsLogic supportMetrics)
        {
            SupportMetricsDal dal = _mapper.mapBackward(supportMetrics);
            return _mapper.mapForward(await _repository.insert(dal));
        }

        private async Task<SupportMetricsLogic> update(SupportMetricsLogic supportMetricsLogic)
        {
            return _mapper.mapForward(
                await _repository.update(
                    supportMetricsLogic.Id, 
                    supportMetricsLogic.ActiveTickets, 
                    supportMetricsLogic.TimesRated, 
                    supportMetricsLogic.Rating
                )
            );
        }

        public async Task<SupportMetricsLogic?> findByIdAsync(Guid id)
        {
            return _mapper.mapForward(await _repository.findByIdAsync(id));
        }

        public async Task<SupportMetricsLogic> findByIdOrThrowAsync(Guid id)
        {
            return _mapper.mapForward(await _repository.findByIdOrThrowAsync(id));
        }

        public async Task<SupportMetricsLogic?> findBySupportIdAsync(Guid supportId)
        {
            return _mapper.mapForward(await _repository.findBySupportIdAsync(supportId));
        }

        public async Task<SupportMetricsLogic> findBySupportIdOrThrowAsync(Guid supportId)
        {
            return _mapper.mapForward(await _repository.findBySupportIdOrThrowAsync(supportId));
        }

        public async Task<Guid> findMostFreeSupportAgent()
        {
            return await _repository.findMostFreeSupportAgent();
        }

        public async Task<bool> deleteByIdAsync(Guid id)
        {
            return await _repository.deleteByIdAsync(id);
        }

        public async Task<SupportMetricsLogic> addActiveTicket(Guid supportId)
        {
            SupportMetricsLogic supportMetricsLogic = await findBySupportIdOrThrowAsync(supportId);
            ++supportMetricsLogic.ActiveTickets;
            return await update(supportMetricsLogic);
        }

        public async Task<SupportMetricsLogic> freeActiveTicket(Guid supportId)
        {
            SupportMetricsLogic supportMetricsLogic = await findBySupportIdOrThrowAsync(supportId);
            
            if (supportMetricsLogic.ActiveTickets == 0)
            {
                return supportMetricsLogic;
            }

            --supportMetricsLogic.ActiveTickets;

            return await update(supportMetricsLogic);
        }

        public async Task<SupportMetricsLogic> addRate(Guid supportId, int rate)
        {
            SupportMetricsLogic supportMetricsLogic = await findBySupportIdOrThrowAsync(supportId);

            List<int> possibleRates = new List<int>(){ 1, 2, 3, 4, 5 };

            if (!possibleRates.Contains(rate))
            {
                throw new ArgumentException("Оценка должна принимать одно из значений: 1, 2, 3, 4, 5.");
            }

            int newTimesRated = supportMetricsLogic.TimesRated + 1;
            double newRating = (supportMetricsLogic.Rating * supportMetricsLogic.TimesRated + rate) / newTimesRated;

            supportMetricsLogic.Rating = newRating;
            supportMetricsLogic.TimesRated = newTimesRated;

            return await update(supportMetricsLogic);
        }
    }
}
