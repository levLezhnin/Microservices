using CoreLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UserApi.Dal.Models;
using UserApi.Logic.Implementations;
using UserApi.Logic.Interfaces;
using UserApi.Logic.Mappers;
using UserApi.Logic.Models;

namespace UserApi.Logic
{
    public static class DalStartUp
    {
        public static IServiceCollection TryAddLogic(this IServiceCollection services)
        {
            services.TryAddScoped<ITwoWayMapper<UserDal, UserLogic>, UserDalLogicMapper>();
            services.TryAddScoped<ITwoWayMapper<UserRoleDal, UserRoleLogic>, UserRoleDalLogicMapper>();
            services.TryAddScoped<ITwoWayMapper<SupportMetricsDal, SupportMetricsLogic>, SupportMetricsDalLogicMapper>();
            services.TryAddScoped<IUserService, UserService>();
            services.TryAddScoped<IUserRoleService, UserRoleService>();
            services.TryAddScoped<ISupportMetricsService, SupportMetricsService>();
            return services;
        }
    }
}
