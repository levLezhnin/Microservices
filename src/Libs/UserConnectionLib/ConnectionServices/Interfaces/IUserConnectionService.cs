using CoreLib.Exceptions;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfo;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfoWithRole;

namespace UserConnectionLib.ConnectionServices.Interfaces
{
    public interface IUserConnectionService
    {
        Task<UserInfoDtoResponse?> GetUserInfo(UserInfoDtoRequest request);
        Task<UserInfoWithRoleResponse?> GetUserInfoWithRole(UserInfoWithRoleRequest request);
        Task<bool> addActiveTicket(Guid supportAgentId);
        Task<Guid?> findMostFreeSupportAgent();
    }
}
