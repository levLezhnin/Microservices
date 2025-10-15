using CoreLib.Dto;

namespace NotificationApi.Domain.Interfaces
{
    public interface IGetUserInfo
    {
        Task<UserInfoDto> getUserInfo(Guid userId);
    }
}
