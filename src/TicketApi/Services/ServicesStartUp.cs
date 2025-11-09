using CoreLib.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services.Implementations;
using Services.Interfaces;
using Services.Mapper;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfo;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfoWithRole;

namespace Services
{
    public static class ServicesStartUp
    {
        public static IServiceCollection TryAddServices(this IServiceCollection services)
        {
            services.TryAddScoped<IAssignTicket, AssignTicket>();
            services.TryAddScoped<IChangeTicketStatus, ChangeTicketStatus>();
            services.TryAddScoped<ICreateTicket, CreateTicket>();
            services.TryAddScoped<IRemoveAgent, RemoveAgent>();
            services.TryAddScoped<IDeleteTicket, DeleteTicket>();
            services.TryAddScoped<IGetTicket, GetTicket>();

            services.TryAddScoped<IMapper<UserInfoDtoResponse?, UserInfo?>, UserInfoMapper>();
            services.TryAddScoped<IMapper<UserInfoWithRoleResponse?, UserInfoWithRole?>, UserInfoWithRoleMapper>();

            return services;
        }
    }
}
