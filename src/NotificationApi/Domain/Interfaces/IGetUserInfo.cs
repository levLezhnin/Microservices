using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfo;

namespace NotificationApi.Domain.Interfaces
{
    public interface IGetUserInfo
    {
        Task<UserInfoDtoResponse> getUserInfo(Guid userId);
    }
}
