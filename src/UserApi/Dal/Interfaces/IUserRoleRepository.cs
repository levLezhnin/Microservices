using CoreLib.Interfaces;
using UserApi.Dal.Models;

namespace UserApi.Dal.Interfaces
{
    public interface IUserRoleRepository : ICrudRepository<UserRoleDal, Guid>
    {
        Task<UserRoleDal?> findByRoleNameAsync(string roleName);
        Task<UserRoleDal> findByRoleNameOrThrowAsync(string roleName);
    }
}
