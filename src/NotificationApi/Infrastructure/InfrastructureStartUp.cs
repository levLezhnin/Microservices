using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NotificationApi.Domain.Interfaces;
using NotificationApi.Infrastructure.Connections.REST;

namespace NotificationApi.Infrastructure
{
    public static class InfrastructureStartUp
    {
        public static IServiceCollection TryAddInfra(this IServiceCollection services)
        {
            services.TryAddScoped<IGetUserInfo, GetUserInfo>();
            services.TryAddScoped<ISender, GmailSender>();
            return services;
        }
    }
}
