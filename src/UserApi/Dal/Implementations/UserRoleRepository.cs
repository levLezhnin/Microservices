using CoreLib.Exceptions;
using CoreLib.Repository;
using Microsoft.EntityFrameworkCore;
using UserApi.Dal.Interfaces;
using UserApi.Dal.Models;

namespace UserApi.Dal.Implementations
{
    public class UserRoleRepository : CrudRepository<UserRoleDal, Guid>, IUserRoleRepository
    {
        public UserRoleRepository(DbContext dbContext) : base(dbContext)
        { }

        public async Task<UserRoleDal?> findByRoleNameAsync(string roleName)
        {
            return await _dbSet.FirstOrDefaultAsync(role => role.Role.Equals(roleName));
        }

        public async Task<UserRoleDal> findByRoleNameOrThrowAsync(string roleName)
        {
            return await _dbSet.FirstOrDefaultAsync(role => role.Role.Equals(roleName)) ??
                throw new EntityNotFoundException($"Роль с названием: {roleName} не найдена!");
        }
    }
}
