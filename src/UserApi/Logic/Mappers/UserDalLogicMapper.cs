using CoreLib.Interfaces;
using UserApi.Dal.Models;
using UserApi.Logic.Models;

namespace UserApi.Logic.Mappers
{
    public class UserDalLogicMapper : ITwoWayMapper<UserDal, UserLogic>
    {
        public UserDal? mapBackward(UserLogic to)
        {
            if (to == null)
            {
                return null;
            }

            return new UserDal
            {
                Id = to.Id,
                FirstName = to.FirstName,
                LastName = to.LastName,
                Email = to.Email,
                PasswordHash = to.PasswordHash,
                Role = to.Role,
                CreatedAt = to.CreatedAt,
                IsActive = to.IsActive
            };
        }

        public UserLogic? mapForward(UserDal from)
        {
            if (from == null)
            {
                return null;
            }

            return new UserLogic
            {
                Id = from.Id,
                FirstName = from.FirstName,
                LastName = from.LastName,
                Email = from.Email,
                PasswordHash = from.PasswordHash,
                Role = from.Role,
                CreatedAt = from.CreatedAt,
                IsActive = from.IsActive
            };
        }
    }
}
