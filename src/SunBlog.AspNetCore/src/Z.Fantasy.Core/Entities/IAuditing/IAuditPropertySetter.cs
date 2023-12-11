using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.Entities.IAuditing
{
    public interface IAuditPropertySetter : ITransientDependency
    {
        void SetCreationProperties(object targetObject);


        void SetDeletionProperties(object targetObject);

    }
}
