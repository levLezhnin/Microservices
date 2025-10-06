using UserApi.Logic.Models;

namespace UserApi.Logic.Interfaces
{
    public interface IUserRoleService
    {
        Task<UserRoleLogic?> findByIdAsync(Guid id);
        Task<UserRoleLogic> findByIdOrThrowAsync(Guid id);
        Task<UserRoleLogic?> findByRoleNameAsync(string roleName);
        Task<UserRoleLogic> findByRoleNameOrThrowAsync(string roleName);
    }
}
