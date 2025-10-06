using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UserApi.Dal.Implementations;
using UserApi.Dal.Interfaces;

namespace UserApi.Dal
{
    public static class DalStartUp
    {
        public static IServiceCollection TryAddDal(this IServiceCollection services)
        {
            services.TryAddScoped<IUserRepository, UserRepository>();
            services.TryAddScoped<IUserRoleRepository, UserRoleRepository>();
            services.TryAddScoped<ISupportMetricsRepository, SupportMetricsRepository>();
            return services;
        }
    }
}
