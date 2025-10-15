using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NotificationApi.Services.Implementations;
using NotificationApi.Services.Interfaces;

namespace NotificationApi.Services
{
    public static class ServicesStartUp
    {
        public static IServiceCollection TryAddServices(this IServiceCollection services)
        {
            services.TryAddScoped<ICreateNotification, CreateNotification>();
            services.TryAddScoped<ISendNotification, SendNotification>();
            return services;
        }
    }
}
