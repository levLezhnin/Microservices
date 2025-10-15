using CoreLib.Dto;
using NotificationApi.Domain.Interfaces;

namespace NotificationApi.Infrastructure.Connections.REST
{
    public class GetUserInfo : IGetUserInfo
    {
        public async Task<UserInfoDto> getUserInfo(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
