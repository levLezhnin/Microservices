using CoreLib.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UserApi.Api.Dto.Mapper;
using UserApi.Api.Dto.Request;
using UserApi.Api.Dto.Response;
using UserApi.Api.Implementations;
using UserApi.Api.Interfaces;
using UserApi.Logic.Models;

namespace UserApi.Api
{
    public static class ControllerStartUp
    {
        public static IServiceCollection TryAddController(this IServiceCollection services)
        {
            services.TryAddScoped<IUserControllerService, UserControllerService>();
            services.TryAddScoped<ISupportMetricsControllerService, SupportMetricsControllerService>();
            services.TryAddScoped<IDtoMapper<CreateUserRequestDto, UserLogic, UserResponseDto>, CreateUserRequestDtoMapper>();
            services.TryAddScoped<IMapper<SupportMetricsLogic, SupportMetricsResponseDto>, SupportMetricsResponseDtoMapper>();
            return services;
        }
    }
}
