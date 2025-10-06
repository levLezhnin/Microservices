using CoreLib.Interfaces;
using UserApi.Api.Dto.Response;
using UserApi.Logic.Models;

namespace UserApi.Api.Dto.Mapper
{
    public class SupportMetricsResponseDtoMapper : IMapper<SupportMetricsLogic, SupportMetricsResponseDto>
    {
        public SupportMetricsResponseDto? map(SupportMetricsLogic from)
        {
            return new SupportMetricsResponseDto
            {
                SupportId = from.SupportId,
                ActiveTickets = from.ActiveTickets,
                TimesRated = from.TimesRated,
                Rating = from.Rating
            };
        }
    }
}
