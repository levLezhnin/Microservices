using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services.Implementations;
using Services.Interfaces;

namespace Services
{
    public static class ServicesStartUp
    {
        public static IServiceCollection TryAddServices(this IServiceCollection services)
        {
            services.TryAddScoped<IChooseSupportAgent, ChooseSupportAgent>();

            return services;
        }
    }
}
