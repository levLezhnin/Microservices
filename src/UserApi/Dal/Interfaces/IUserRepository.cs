using CoreLib.Interfaces;
using UserApi.Dal.Models;

namespace UserApi.Dal.Interfaces
{
    public interface IUserRepository : ICrudRepository<UserDal, Guid>
    {
        Task<UserDal> update(Guid id, string? firstName, string? lastName, bool? isActive);
        Task<UserDal?> findByEmailAsync(string email);
        Task<UserDal> findByEmailOrThrowAsync(string email);
    }
}
