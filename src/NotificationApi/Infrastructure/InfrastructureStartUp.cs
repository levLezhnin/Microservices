using CoreLib.HttpServiceV2.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NotificationApi.Domain.Interfaces;
using NotificationApi.Infrastructure.Connections.REST;
using UserConnectionLib.ConnectionServices;
using UserConnectionLib.ConnectionServices.Interfaces;

namespace NotificationApi.Infrastructure
{
    public static class InfrastructureStartUp
    {
        public static IServiceCollection TryAddInfra(this IServiceCollection services)
        {
            services.TryAddScoped<IGetUserInfo, GetUserInfo>();
            services.TryAddScoped<ISender, GmailSender>();
            services.TryAddScoped<IUserConnectionService, UserConnectionService>();
            return services;
        }
    }
}
