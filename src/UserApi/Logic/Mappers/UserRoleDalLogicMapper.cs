using CoreLib.Interfaces;
using UserApi.Dal.Models;
using UserApi.Logic.Models;

namespace UserApi.Logic.Mappers
{
    public class UserRoleDalLogicMapper : ITwoWayMapper<UserRoleDal, UserRoleLogic>
    {

        public UserRoleDal? mapBackward(UserRoleLogic to)
        {
            if (to == null)
            {
                return null;
            }

            return new UserRoleDal {
                Id = to.Id,
                Role = to.Role 
            };
        }

        public UserRoleLogic? mapForward(UserRoleDal from)
        {
            if (from == null)
            {
                return null;
            }

            return new UserRoleLogic
            {
                Id = from.Id,
                Role = from.Role
            };
        }
    }
}
