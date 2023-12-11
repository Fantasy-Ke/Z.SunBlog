using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Entities;

namespace Z.EntityFrameworkCore.Core;

public class Repository<TDbContext, TEntity> : EfCoreRepository<TDbContext, TEntity>
    where TEntity : class, IEntity
    where TDbContext : DbContext
{
    public Repository(TDbContext dbContext) : base(dbContext)
    {
    }
}


public class Repository<TDbContext, TEntity, TKey> : EfCoreRepository<TDbContext, TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TDbContext : DbContext
{
    public Repository(TDbContext dbContext) : base(dbContext)
    {
    }
}