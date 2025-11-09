using CoreLib.HttpServiceV2.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserConnectionLib.ConnectionServices;
using UserConnectionLib.ConnectionServices.Interfaces;

namespace Infrastructure
{
    public static class InfrastructureStartUp
    {
        public static IServiceCollection TryAddInfra(this IServiceCollection services)
        {
            services.TryAddScoped<IUserConnectionService, UserConnectionService>();

            return services;
        }
    }
}
