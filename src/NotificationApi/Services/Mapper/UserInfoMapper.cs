using CoreLib.Interfaces;
using NotificationApi.Domain.Entities;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfo;

namespace NotificationApi.Services.Mapper
{
    public class UserInfoMapper : IMapper<UserInfoDtoResponse, UserInfo>
    {
        public UserInfo? map(UserInfoDtoResponse from)
        {
            if (from == null)
            {
                return null;
            }

            return new UserInfo
            {
                firstName = from.firstName,
                lastName = from.lastName,
                email = from.email
            };
        }
    }
}
