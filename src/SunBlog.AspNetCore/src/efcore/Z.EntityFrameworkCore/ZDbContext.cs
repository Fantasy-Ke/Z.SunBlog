using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Reflection;
using Z.Ddd.Common.Entities.EntityLog;
using Z.Ddd.Common.Entities.IAuditing;
using Z.Ddd.Common.Entities.Organizations;
using Z.Ddd.Common.Entities.Roles;
using Z.Ddd.Common.Entities.Users;
using Z.Ddd.Common.Entities.Files;
using Z.EntityFrameworkCore.Extensions;
using Z.EntityFrameworkCore.Options;
using Z.Ddd.Common.Entities.Permission;

namespace Z.EntityFrameworkCore
{
    public abstract class ZDbContext : DbContext, IDisposable
    {
        protected ZDbContextOptions? Options { get; private set; }
        public virtual DbSet<ZUserInfo> ZUsers { get; set; }
        public virtual DbSet<ZRoleInfo> ZRoles { get; set; }
        public virtual DbSet<ZOrganization> ZOrganizations { get; set; }
        public virtual DbSet<ZUserRole> ZUserRoles { get; set; }
        public virtual DbSet<ZAccessLog> ZAccessLog { get; set; }
        public virtual DbSet<ZOperationLog> ZOperationLog { get; set; }
        public virtual DbSet<ZRequestLog> ZRequestLog { get; set; }
        public virtual DbSet<ZFileInfo> ZFileInfo { get; set; }
        public virtual DbSet<ZPermissions> ZPermissions { get; set; }


        protected ZDbContext(ZDbContextOptions options)
        : base(options)
        {
            Options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ConfigureSoftDelete(modelBuilder);
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddZCoreConfigure();

            OnModelCreatingConfigureGlobalFilters(modelBuilder);
        }

        protected sealed override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Options == null)
                return;

            var zDbContextOptionsBuilder = new ZDbContextOptionsBuilder(optionsBuilder, Options);
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(
                Console.WriteLine,
                LogLevel.Warning,
                DbContextLoggerOptions.UtcTime | DbContextLoggerOptions.SingleLine);
        }


        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }



        protected virtual bool IsSoftDeleteFilterEnabled
        => Options is { EnableSoftDelete: true };



        protected virtual void OnModelCreatingConfigureGlobalFilters(ModelBuilder modelBuilder)
        {
            var methodInfo = GetType().GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                methodInfo!.MakeGenericMethod(entityType.ClrType).Invoke(this, new object?[]
                {
                modelBuilder, entityType
                });
            }
        }

        /// <summary>
        /// Filters
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="mutableEntityType"></param>

        protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        where TEntity : class
        {
            if (mutableEntityType.BaseType == null)
            {
                var filterExpression = CreateFilterExpression<TEntity>();
                if (filterExpression != null)
                    modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
            }
        }

        /// <summary>
        /// 过滤Expression 软删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        protected virtual Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>()
        where TEntity : class
        {
            Expression<Func<TEntity, bool>>? expression = null;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                expression = entity => !IsSoftDeleteFilterEnabled || !EF.Property<bool>(entity, nameof(ISoftDelete.IsDeleted));
            }
            return expression;
        }

        internal void TryInitializeZDbContextOptions(ZDbContextOptions? options)
        {

            try
            {
                _ = base.ChangeTracker;
            }
            catch (InvalidOperationException ex)
            {
                ILogger? logger = null;
                if (options != null)
                {
                    var loggerType = typeof(ILogger<>).MakeGenericType(options.ContextType);
                    logger = options.ServiceProvider?.GetService(loggerType) as ILogger;
                }

                logger ??= Options?.ServiceProvider?.GetService<ILogger<ZDbContext>>();
                logger?.LogDebug(ex, "Error generating data context");

                if (ex.Message.Contains("overriding the 'DbContext.OnConfiguring'"))
                    throw new InvalidOperationException("No database provider has been configured for this DbContext. A provider can be configured by overriding the 'MasaDbContext.OnConfiguring' method or by using 'AddMasaDbContext' on the application service provider. If 'AddMasaDbContext' is used, then also ensure that your DbContext type accepts a 'MasaDbContextOptions<TContext>' object in its constructor and passes it to the base constructor for DbContext.", ex);
                throw;
            }
        }

    }


    public abstract class ZDbContext<TDbContext> : ZDbContext
    where TDbContext : ZDbContext<TDbContext>
    {
        protected ZDbContext() : base(new ZDbContextOptions<ZDbContext>())
        {
        }

        protected ZDbContext(ZDbContextOptions<TDbContext> options) : base(options)
        {
        }
    }
}
