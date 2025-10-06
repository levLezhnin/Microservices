using CoreLib.Interfaces;
using UserApi.Api.Dto.Response;
using UserApi.Api.Interfaces;
using UserApi.Logic.Interfaces;
using UserApi.Logic.Models;

namespace UserApi.Api.Implementations
{
    public class SupportMetricsControllerService : ISupportMetricsControllerService
    {
        private readonly ISupportMetricsService _supportMetricsService;
        private readonly IMapper<SupportMetricsLogic, SupportMetricsResponseDto> _mapper;

        public SupportMetricsControllerService(ISupportMetricsService supportMetricsService, IMapper<SupportMetricsLogic, SupportMetricsResponseDto> mapper)
        {
            _supportMetricsService = supportMetricsService;
            _mapper = mapper;
        }

        public async Task<SupportMetricsResponseDto> findBySupportIdOrThrowAsync(Guid supportId)
        {
            return _mapper.map(await _supportMetricsService.findBySupportIdOrThrowAsync(supportId));
        }

        public async Task<SupportMetricsResponseDto> addActiveTicket(Guid supportId)
        {
            return _mapper.map(await _supportMetricsService.addActiveTicket(supportId));
        }

        public async Task<SupportMetricsResponseDto> addRate(Guid supportId, int rating)
        {
            return _mapper.map(await _supportMetricsService.addRate(supportId, rating));
        }

        public async Task<Guid> findMostFreeSupportAgent()
        {
            return await _supportMetricsService.findMostFreeSupportAgent();
        }

        public async Task<SupportMetricsResponseDto> freeActiveTicket(Guid supportId)
        {
            return _mapper.map(await _supportMetricsService.freeActiveTicket(supportId));
        }
    }
}
