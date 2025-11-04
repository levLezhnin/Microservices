using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfo;

namespace Domain.Interfaces
{
    public interface IGetUserInfo
    {
        Task<UserInfoDtoResponse?> getUserInfo(Guid userId);
    }
}
