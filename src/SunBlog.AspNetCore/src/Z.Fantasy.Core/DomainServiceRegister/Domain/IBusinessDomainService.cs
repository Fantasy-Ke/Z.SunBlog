using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Entities;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.DomainServiceRegister.Domain;

public interface IBusinessDomainService<TEntity> : IBasicDomainService<TEntity, Guid>, IDomainService, ITransientDependency where TEntity : class, IEntity<Guid>
{
    Guid NewGuid();
}
