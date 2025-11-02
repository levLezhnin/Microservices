using CoreLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NotificationApi.Domain.Entities;
using NotificationApi.Services.Implementations;
using NotificationApi.Services.Interfaces;
using NotificationApi.Services.Mapper;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfo;

namespace NotificationApi.Services
{
    public static class ServicesStartUp
    {
        public static IServiceCollection TryAddServices(this IServiceCollection services)
        {
            services.TryAddScoped<ICreateNotification, CreateNotification>();
            services.TryAddScoped<ISendNotification, SendNotification>();
            services.TryAddScoped<IMapper<UserInfoDtoResponse, UserInfo>, UserInfoMapper>();
            return services;
        }
    }
}
