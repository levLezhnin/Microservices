using CoreLib.Interfaces;
using UserApi.Dal.Models;
using UserApi.Logic.Models;

namespace UserApi.Logic.Mappers
{
    public class SupportMetricsDalLogicMapper : ITwoWayMapper<SupportMetricsDal, SupportMetricsLogic>
    {

        public SupportMetricsDal? mapBackward(SupportMetricsLogic to)
        {
            if (to == null)
            {
                return null;
            }

            return new SupportMetricsDal
            {
                Id = to.Id,
                SupportId = to.SupportId,
                ActiveTickets = to.ActiveTickets,
                TimesRated = to.TimesRated,
                Rating = to.Rating
            };
        }

        public SupportMetricsLogic? mapForward(SupportMetricsDal from)
        {
            if (from == null)
            {
                return null;
            }

            return new SupportMetricsLogic
            {
                Id = from.Id,
                SupportId = from.SupportId,
                ActiveTickets = from.ActiveTickets,
                TimesRated = from.TimesRated,
                Rating = from.Rating
            };
        }
    }
}
