using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Entities.Repositories;
using Z.Fantasy.Core.Entities;
using Z.EntityFrameworkCore.Core;

namespace Z.SunBlog.EntityFrameworkCore.EntityFrameworkCore.Repositories;

public abstract class FrameWorkRepositoryBase<TEntity, TKey> : EfCoreRepository<SunBlogDbContext, TEntity, TKey>
        where TEntity : class, IEntity<TKey>
{
    protected FrameWorkRepositoryBase(SunBlogDbContext dbContextProvider)
        : base(dbContextProvider)
    {
    }

}

public abstract class FrameWorkRepositoryBase<TEntity> : FrameWorkRepositoryBase<TEntity, int>, IRepository<TEntity>
    where TEntity : class, IEntity<int>
{
    protected FrameWorkRepositoryBase(SunBlogDbContext dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
