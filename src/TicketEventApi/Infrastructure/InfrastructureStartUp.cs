using Contracts.Messages;
using Domain.Interfaces;
using Infrastructure.Messaging.Saga;
using Infrastructure.Repository;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure
{
    public static class InfrastructureStartUp
    {
        public static IServiceCollection TryAddInfra(this IServiceCollection services)
        {
            services.TryAddScoped<ITicketEventRepository, TicketEventRepository>();

            return services;
        }
    }
}
