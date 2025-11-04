using CoreLib.Common;
using CoreLib.Interfaces;
using Domain.Entities;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfoWithRole;

namespace Services.Mapper
{
    public class UserInfoWithRoleMapper : IMapper<UserInfoWithRoleResponse?, UserInfoWithRole?>
    {
        public UserInfoWithRole? map(UserInfoWithRoleResponse? from)
        {
            if (from is null)
            {
                return null;
            }

            return new UserInfoWithRole
            {
                firstName = from.firstName,
                lastName = from.lastName,
                email = from.email,
                role = Enum.Parse<UserRoles>(from.role)
            };
        }
    }
}
