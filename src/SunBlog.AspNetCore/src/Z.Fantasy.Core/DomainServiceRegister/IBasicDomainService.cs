using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Z.Module.DependencyInjection;
using Z.Fantasy.Core.DomainServiceRegister.Domain;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.Fantasy.Core.Entities;
using Z.Fantasy.Core.Entities.Repositories;

namespace Z.Fantasy.Core.DomainServiceRegister;

public interface IBasicDomainService<TEntity, TPrimaryKey> : IDomainService, ITransientDependency where TEntity : class, IEntity<TPrimaryKey>
{
    //
    // 摘要:
    //     实体
    IBasicRepository<TEntity, TPrimaryKey> EntityRepo { get; }

    //
    // 摘要:
    //     查询器
    IQueryable<TEntity> Query { get; }

    //
    // 摘要:
    //     查询器 - 不追踪
    IQueryable<TEntity> QueryAsNoTracking { get; }

    //
    // 摘要:
    //     根据id查找
    //
    // 参数:
    //   id:
    Task<TEntity> FindByIdAsync(TPrimaryKey id);

    //
    // 摘要:
    //     创建
    //
    // 参数:
    //   entity:
    //
    //   createAndGetId:
    //     是否获取id
    Task<TEntity> CreateAsync(TEntity entity);

    //
    // 摘要:
    //     批量创建
    //
    // 参数:
    //   entities:
    //     实体对象
    //
    //   createAndGetId:
    //     是否获取id
    Task CreateAsync(IEnumerable<TEntity> entities);


    Task<PageResult<TEntity>> ToPagedListAsync(IPagination input);

    //
    // 摘要:
    //     更新
    //
    // 参数:
    //   entity:
    Task<TEntity> UpdateAsync(TEntity entity);

    //
    // 摘要:
    //     更新
    //
    // 参数:
    //   entities:
    //     实体对象
    Task UpdateAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="columns">修改的类字段</param>
    /// <param name="whereExpression">条件</param>
    /// <returns></returns>

    Task<TEntity> UpdateAsync(TEntity columns, Expression<Func<TEntity, bool>> whereExpression);

    //
    // 摘要:
    //     删除
    //
    // 参数:
    //   id:
    Task DeleteAsync(TPrimaryKey id);

    //
    // 摘要:
    //     删除
    //
    // 参数:
    //   entity:
    Task DeleteAsync(TEntity entity);

    //
    // 摘要:
    //     删除 - 按条件
    //
    // 参数:
    //   predicate:
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    //
    // 摘要:
    //     批量删除
    //
    // 参数:
    //   idList:
    Task DeleteAsync(List<TPrimaryKey> idList);

    //
    // 摘要:
    //     批量删除
    //
    // 参数:
    //   idList:
    Task BatchDeleteAsync();

    //
    // 摘要:
    //     是否存在
    //
    // 参数:
    //   id:
    Task<bool> ExistAsync(TPrimaryKey id);

    //
    // 摘要:
    //     创建或更新
    //
    // 参数:
    //   entity:
    Task<TEntity> CreateOrUpdateAsync(Expression<Func<TEntity, bool>> predicate,
        TEntity entity);

    //
    // 摘要:
    //     批量创建或更新
    //
    // 参数:
    //   entities:
    Task CreateOrUpdateAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<TEntity> entities);



    Task<bool> IsAnyAsync(Expression<Func<TEntity, bool>> predicate);
}
