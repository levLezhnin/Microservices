using CoreLib.Common;
using CoreLib.Exceptions;
using CoreLib.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreLib.Repository
{
    public abstract class CrudRepository<TEntity, TId> : ICrudRepository<TEntity, TId>
        where TEntity : BaseEntityDal<TId>
        where TId : IEquatable<TId>
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        protected CrudRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity> insert(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TEntity> update(TEntity entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TEntity?> findByIdAsync(TId id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<TEntity> findByIdOrThrowAsync(TId id)
        {
            return await _dbSet.FindAsync(id) ??
                throw new EntityNotFoundException($"Сущность с id: {id} не найдена!");
        }

        public virtual async Task<bool> deleteByIdAsync(TId id)
        {
            var entity = await findByIdOrThrowAsync(id);

            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
