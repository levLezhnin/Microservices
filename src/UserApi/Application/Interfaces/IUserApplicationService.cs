using UserApi.Logic.Models;

namespace UserApi.Application.Interfaces
{
    public interface IUserApplicationService
    {
        Task<UserLogic> insert(
            string firstName, 
            string lastName, 
            string email, 
            string passwordHash,
            string role
        );

        Task<UserLogic> update(
            Guid id,
            string firstName,
            string lastName,
            string email,
            string passwordHash,
            string role
        );

        Task<UserLogic?> findByIdAsync(Guid id);
        Task<UserLogic> findByIdOrThrowAsync(Guid id);

        Task<UserLogic?> findByEmailAsync(string email);
        Task<UserLogic> findByEmailOrThrowAsync(string email);
    }
}
