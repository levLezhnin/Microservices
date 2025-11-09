using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfoWithRole;

namespace Domain.Interfaces
{
    public interface IGetUserInfoWithRole
    {
        Task<UserInfoWithRoleResponse?> getUserInfoWithRole(Guid userId);
    }
}
