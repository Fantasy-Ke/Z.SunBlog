using Cuemon;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Z.Fantasy.Core.Entities;
using Z.Fantasy.Core.Entities.Repositories;
using Z.Fantasy.Core.ResultResponse;
using Z.Module.DependencyInjection;

namespace Z.EntityFrameworkCore.Core;

/// <summary>
/// 通用仓储实现
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class EfCoreRepository<TDbContext, TEntity, TKey> : EfCoreRepository<TDbContext, TEntity>,
    IBasicRepository<TEntity, TKey>
    where TEntity :class , IEntity<TKey>
    where TDbContext : DbContext
{
    protected EfCoreRepository(TDbContext dbContext) : base(dbContext)
    {
    }

    public async Task DeleteIDAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await DbSet.FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken: cancellationToken);
        if (entity != null)
        {
            DbSet.Remove(entity);
        }
    }

    public async Task DeleteManyAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
    {
        var entity = await DbSet.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken: cancellationToken);

        if (entity.Count > 0)
        {
            DbSet.RemoveRange(entity);
        }
    }

    public async Task<TEntity?> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default) 
        => await DbSet.FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken: cancellationToken);

    public async Task<TEntity?> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default) 
        => await DbSet.FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken: cancellationToken);
}

public abstract class EfCoreRepository<TDbContext, TEntity> : IBasicRepository<TEntity>, ITransientDependency
    where TEntity : class, IEntity
    where TDbContext : DbContext
{
    protected readonly DbSet<TEntity> DbSet;

    protected EfCoreRepository(TDbContext dbContext)
    {
        DbSet = dbContext.Set<TEntity>();
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }

    public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstAsync(predicate, cancellationToken: cancellationToken);
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.CountAsync(predicate, cancellationToken) > 0;
    }

    public async Task<IQueryable<TEntity>> GetQueryAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Task.FromResult(DbSet.Where(predicate));
    }

    public async Task<IQueryable<TResult>> GetQueryAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector)
    {
        return await Task.FromResult(DbSet.Where(predicate).Select(selector));
    }

    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return (await DbSet.AddAsync(entity, cancellationToken)).Entity;
    }

    public TEntity Insert(TEntity entity)
    {
        return DbSet.Add(entity).Entity;
    }

    public async Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await DbSet.AddRangeAsync(entities, cancellationToken);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteManyAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default)
    {
        DbSet.RemoveRange(entity);
        return Task.CompletedTask;
    }

    public Task DeleteManyAsync(CancellationToken cancellationToken = default)
    {
        DbSet.RemoveRange(DbSet);
        return Task.CompletedTask;
    }


    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        return await Task.FromResult(entity);
    }

    public Task UpdateManyAsync(IEnumerable<TEntity> entities)
    {
        DbSet.UpdateRange(entities);

        return Task.CompletedTask;
    }

    public Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<(List<TEntity>, int)> GetPagedListAsync(int pageNumber,int pageSize, CancellationToken cancellationToken = default)
    {
        List<TEntity> list = await DbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return (list, await DbSet.CountAsync());
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entityList = await DbSet.Where(predicate).ToListAsync();
        if (entityList != null && entityList.Any())
        {
            DbSet.RemoveRange(entityList);
        }
    }

    public IQueryable<TEntity> GetQueryAll()
    {
        return DbSet.AsQueryable();
    }

    public async Task<TEntity> InsertOrUpdateAsync(Expression<Func<TEntity, bool>> predicate,
        TEntity entity)
    {
        var existingEntity = await DbSet.FirstOrDefaultAsync(predicate);
        if (existingEntity == null)
        {
            DbSet.Add(entity); // 如果记录不存在，则新增
        }
        else
        {
            // 如果记录存在，则进行更新
            DbSet.Attach(existingEntity);
            DbSet.Update(entity);
        }

        return await Task.FromResult(entity); ;
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return  await DbSet.CountAsync(predicate);
    }
}

