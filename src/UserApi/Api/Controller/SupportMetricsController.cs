using Microsoft.AspNetCore.Mvc;
using UserApi.Api.Dto.Response;
using UserApi.Api.Interfaces;

namespace UserApi.Api.Controller
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class SupportMetricsController
    {
        private readonly ISupportMetricsControllerService _supportMetricsControllerService;

        public SupportMetricsController(ISupportMetricsControllerService supportMetricsControllerService)
        {
            _supportMetricsControllerService = supportMetricsControllerService;
        }

        [HttpGet("agent/{supportId}")]
        public async Task<SupportMetricsResponseDto> findMetricsBySupportId([FromRoute] Guid supportId)
        {
            return await _supportMetricsControllerService.findBySupportIdOrThrowAsync(supportId);
        }

        [HttpGet("free")]
        public async Task<Guid> findMostFreeAgent()
        {
            return await _supportMetricsControllerService.findMostFreeSupportAgent();
        }

        [HttpPatch("agent/{supportId}/addTicket")]
        public async Task<SupportMetricsResponseDto> addActiveTicket([FromRoute] Guid supportId)
        {
            return await _supportMetricsControllerService.addActiveTicket(supportId);
        }

        [HttpPatch("agent/{supportId}/freeTicket")]
        public async Task<SupportMetricsResponseDto> freeActiveTicket([FromRoute] Guid supportId)
        {
            return await _supportMetricsControllerService.freeActiveTicket(supportId);
        }

        [HttpPatch("agent/{supportId}/addRate")]
        public async Task<SupportMetricsResponseDto> addRate([FromRoute] Guid supportId, [FromQuery] int rate)
        {
            return await _supportMetricsControllerService.addRate(supportId, rate);
        }
    }
}
