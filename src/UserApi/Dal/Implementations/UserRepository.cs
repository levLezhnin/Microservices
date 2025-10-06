using CoreLib.Exceptions;
using CoreLib.Repository;
using Microsoft.EntityFrameworkCore;
using UserApi.Dal.Interfaces;
using UserApi.Dal.Models;

namespace UserApi.Dal.Implementations
{
    public class UserRepository : CrudRepository<UserDal, Guid>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext)
        {}

        public async Task<UserDal> update(Guid id, string? firstName, string? lastName, bool? isActive)
        {
            UserDal userDal = await findByIdOrThrowAsync(id);
            if (firstName != null)
            {
                userDal.FirstName = firstName;
            }
            if (lastName != null)
            {
                userDal.LastName = lastName;
            }
            if (isActive.HasValue)
            {
                userDal.IsActive = isActive.Value;
            }
            return await update(userDal);
        } 

        public async Task<UserDal?> findByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(user => user.Email.Equals(email));
        }

        public async Task<UserDal> findByEmailOrThrowAsync(string email)
        {
            UserDal dal = await _dbSet.FirstOrDefaultAsync(user => user.Email.Equals(email));
            Console.WriteLine(dal == null);
            Console.WriteLine(dal);
            if (dal == null)
            {
                throw new EntityNotFoundException($"Пользователь с email: {email} не найден!");
            }
            return dal;
        }
    }
}
