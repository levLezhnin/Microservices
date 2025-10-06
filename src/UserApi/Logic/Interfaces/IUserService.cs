using UserApi.Logic.Models;

namespace UserApi.Logic.Interfaces
{
    public interface IUserService
    {
        Task<UserLogic> insert(UserLogic userLogic);

        Task<UserLogic> update(Guid id, string? firstName, string? lastName, bool? isActive);

        Task<UserLogic?> findByIdAsync(Guid id);
        Task<UserLogic> findByIdOrThrowAsync(Guid id);

        Task<UserLogic?> findByEmailAsync(string email);
        Task<UserLogic> findByEmailOrThrowAsync(string email);
    }
}
