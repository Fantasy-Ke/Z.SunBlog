﻿using Z.Foundation.Core.Entities;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.DomainServiceRegister.Domain;

public abstract class BusinessDomainService<TEntity> : BasicDomainService<TEntity, Guid>,
    IBusinessDomainService<TEntity>,
    IDomainService, ITransientDependency where TEntity : class, IEntity<Guid>
{
    protected BusinessDomainService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public Guid NewGuid()
    {
        return Guid.NewGuid();
    }
}
