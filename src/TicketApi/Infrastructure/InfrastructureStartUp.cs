using Domain.Interfaces;
using Infrastructure.Messaging.REST;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UserConnectionLib.ConnectionServices;
using UserConnectionLib.ConnectionServices.Interfaces;

namespace Infrastructure
{
    public static class InfrastructureStartUp
    {
        public static IServiceCollection TryAddInfra(this IServiceCollection services)
        {
            services.TryAddScoped<ITicketRepository, TicketRepository>();

            services.TryAddScoped<IGetUserInfo, GetUserInfo>();
            services.TryAddScoped<IGetUserInfoWithRole, GetUserInfoWithRole>();

            services.TryAddScoped<IUserConnectionService, UserConnectionService>();

            return services;
        }
    }
}
