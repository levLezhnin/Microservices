using CoreLib.Common;

namespace CoreLib.Interfaces
{
    public interface ICrudRepository<TEntity, TId>
        where TEntity : BaseEntityDal<TId>
        where TId : IEquatable<TId>
    {
        Task<TEntity> insert(TEntity entity);

        Task<TEntity> update(TEntity entity);

        Task<TEntity?> findByIdAsync(TId id);
        Task<TEntity> findByIdOrThrowAsync(TId id);

        Task<bool> deleteByIdAsync(TId id);
    }
}
