using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Entities.IAuditing;
using Z.Fantasy.Core.UnitOfWork;
using Z.EntityFrameworkCore.Options;
using Z.Module.DependencyInjection;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities;
using Z.Fantasy.Core.Extensions;
using Z.Fantasy.Core.Helper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Internal;

namespace Z.EntityFrameworkCore.Core;

public sealed class UnitOfWork<TDbContext> : IUnitOfWork, IDisposable where TDbContext : ZDbContext<TDbContext>
{
    public bool IsDisposed { get; private set; }

    public bool IsRollback { get; private set; }
    public bool IsCompleted { get; private set; }

    private readonly TDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly UnitOfWorkOptions _unitOfWorkOptions;
    private readonly IAuditPropertySetter _auditPropertySetter;

    public UnitOfWork(TDbContext dbContext, IAuditPropertySetter auditPropertySetter)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException($"db context nameof{nameof(dbContext)} is null");
        //_serviceProvider = serviceProvider;
        //_unitOfWorkOptions = unitOfWorkOptions.Value;
        _auditPropertySetter = auditPropertySetter;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        IsCompleted = false;
        return await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (IsCompleted)
        {
            return;
        }

        IsCompleted = true;
        ApplyChangeConventions();
        try
        {
            // 提交事务
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await _dbContext.Database.CommitTransactionAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception)
        {
            // 发生异常回滚事务
            await RollbackTransactionAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (IsCompleted)
        {
            return;
        }
        IsRollback = true;
        await _dbContext.Database.RollbackTransactionAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyChangeConventions();
        return await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private void ApplyChangeConventions()
    {
        _dbContext.ChangeTracker.DetectChanges();
        foreach (var entry in _dbContext.ChangeTracker.Entries())
        {
            PublishEventsForTrackedEntity(entry);
        }
    }

    private void PublishEventsForTrackedEntity(EntityEntry entry)
    {
        switch (entry.State)
        {
            case EntityState.Added:
                ApplyAbpConceptsForAddedEntity(entry);
                break;
            case EntityState.Modified:

                break;
            case EntityState.Deleted:
                ApplyAbpConceptsForDeletedEntity(entry);
                break;
        }
    }


    private void ApplyAbpConceptsForAddedEntity(EntityEntry entry)
    {
        CheckAndSetId(entry);
        SetCreationAuditProperties(entry);
    }

    private void ApplyAbpConceptsForDeletedEntity(EntityEntry entry)
    {
        if (!(entry.Entity is ISoftDelete))
        {
            return;
        }


        entry.Reload();
        ObjectPropertyHelper.TrySetProperty(entry.Entity.As<ISoftDelete>(), x => x.IsDeleted, () => true);
        SetDeletionAuditProperties(entry);
    }


    private void SetCreationAuditProperties(EntityEntry entry)
    {
        _auditPropertySetter?.SetCreationProperties(entry.Entity);
    }

    private void SetDeletionAuditProperties(EntityEntry entry)
    {
        _auditPropertySetter?.SetDeletionProperties(entry.Entity);
    }

    private void CheckAndSetId(EntityEntry entry)
    {
        if (entry.Entity is IEntity<Guid> entityWithGuidId)
        {
            TrySetGuidId(entry, entityWithGuidId);
        }

        if (entry.Entity is IEntity<string> entityWithStringId)
        {
            TrySetStringId(entry, entityWithStringId);
        }
    }


    private void TrySetGuidId(EntityEntry entry, IEntity<Guid> entity)
    {
        if (entity.Id != default)
        {
            return;
        }

        var idProperty = entry.Property("Id").Metadata.PropertyInfo;


        EntityHelper.TrySetId(
            entity,
            () => Guid.NewGuid(),
            true
        );
    }


    private void TrySetStringId(EntityEntry entry, IEntity<string> entity)
    {
        if (!string.IsNullOrWhiteSpace(entity.Id) && entity.Id?.Length <= 32)
        {
            return;
        }

        var idProperty = entry.Property("Id").Metadata.PropertyInfo;


        EntityHelper.TrySetId(
            entity,
            () => Guid.NewGuid().ToString("N"),
            true
        );
    }

    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        IsDisposed = true;
    }

}
