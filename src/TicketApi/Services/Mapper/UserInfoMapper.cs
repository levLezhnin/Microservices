using CoreLib.Interfaces;
using Domain.Entities;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfo;

namespace Services.Mapper
{
    public class UserInfoMapper : IMapper<UserInfoDtoResponse?, UserInfo?>
    {
        public UserInfo? map(UserInfoDtoResponse? from)
        {
            if (from is null)
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
