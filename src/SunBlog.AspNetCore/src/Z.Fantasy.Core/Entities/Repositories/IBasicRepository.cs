using JetBrains.Annotations;
using System.Linq.Expressions;
using Z.Fantasy.Core.Entities;

namespace Z.Fantasy.Core.Entities.Repositories;

public interface IBasicRepository<TEntity> : IReadOnlyBasicRepository<TEntity>
        where TEntity : class, IEntity
{
    /// <summary>
    /// Inserts a new entity.
    /// </summary>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <param name="entity">Inserted entity</param>
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);


    TEntity Insert(TEntity entity);

    /// <summary>
    /// Inserts multiple new entities.
    /// </summary>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <param name="entities">Entities to be inserted.</param>
    /// <returns>Awaitable <see cref="Task"/>.</returns>
    Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <param name="entity">Entity</param>

    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// Updates multiple entities.
    /// </summary>
    /// <param name="entities">Entities to be updated.</param>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Awaitable <see cref="Task"/>.</returns>
    Task UpdateManyAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="entity">Entity to be deleted</param>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="entity">Entity to be deleted</param>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Deletes multiple entities.
    /// </summary>
    /// <param name="entities">Entities to be deleted.</param>
    /// <param name="autoSave">
    /// Set true to automatically save changes to database.
    /// This is useful for ORMs / database APIs those only save changes with an explicit method call, but you need to immediately save changes to the database.
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Awaitable <see cref="Task"/>.</returns>
    Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取到IQueryable
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    IQueryable<TEntity> GetQueryAll();

    //
    // 摘要:
    //     Inserts or updates given entity depending on Id's value.
    //
    // 参数:
    //   entity:
    //     Entity
    Task<TEntity> InsertOrUpdateAsync(Expression<Func<TEntity, bool>> predicate,
        TEntity entity);

}

public interface IBasicRepository<TEntity, TKey> : IBasicRepository<TEntity>, IReadOnlyBasicRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{


    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteIDAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteManyAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteManyAsync(CancellationToken cancellationToken = default);
}