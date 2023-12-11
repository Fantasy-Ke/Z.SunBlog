using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Entities;

namespace Z.Fantasy.Core.Entities.Repositories;

public interface IRepository<TEntity, TKey> : IReadOnlyBasicRepository<TEntity, TKey>, IBasicRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
{

}

public interface IRepository
{

}

public interface IRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// 返回满足条件的第一个元素
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 返回满足条件的第一个元素不存在抛出异常
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取到IQueryable
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    IQueryable<TEntity> GetQueryAll();


    /// <summary>
    /// 获取到IQueryable
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IQueryable<TEntity>> GetQueryAsync(Expression<Func<TEntity, bool>> predicate);


    Task<IQueryable<TResult>> GetQueryAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector);

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量添加
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteManyAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// 批量更新
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task UpdateManyAsync(IEnumerable<TEntity> entities);
}
