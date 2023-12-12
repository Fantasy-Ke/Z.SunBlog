using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.EntityFrameworkCore.Options
{
    public class ZDbContextOptionsBuilder
    {
        internal IServiceProvider? ServiceProvider { get; }

        internal bool EnableSoftDelete { get; }

        public DbContextOptionsBuilder DbContextOptionsBuilder { get; }

        public Type DbContextType { get; }

        public ZDbContextOptionsBuilder(ZDbContextOptions masaDbContextOptions, Func<DbContextOptionsBuilder>? configure = null)
        {
            ServiceProvider = masaDbContextOptions.ServiceProvider;
            EnableSoftDelete = masaDbContextOptions.EnableSoftDelete;
            DbContextType = masaDbContextOptions.DbContextType;
            DbContextOptionsBuilder = configure == null ? new DbContextOptionsBuilder(masaDbContextOptions) : configure.Invoke();
        }

        public ZDbContextOptionsBuilder(DbContextOptionsBuilder dbContextOptionsBuilder, ZDbContextOptions masaDbContextOptions)
            : this(masaDbContextOptions, () => dbContextOptionsBuilder)
        {
        }
    }


    public class ZDbContextOptionsBuilder<TDbContext> : ZDbContextOptionsBuilder
    where TDbContext : DbContext
    {
        public ZDbContextOptions<TDbContext> ZOptions
            => new(ServiceProvider, DbContextOptionsBuilder.Options, EnableSoftDelete);

        public ZDbContextOptionsBuilder(bool enableSoftDelete = false) : this(null, enableSoftDelete)
        {
        }

        public ZDbContextOptionsBuilder(
            IServiceProvider? serviceProvider,
            bool enableSoftDelete = false)
            : base(new ZDbContextOptions<TDbContext>(serviceProvider, new DbContextOptions<TDbContext>(), enableSoftDelete))
        {
        }
    }

}
