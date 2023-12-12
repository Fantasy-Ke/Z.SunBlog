using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Aop.AopLog;

namespace Z.Fantasy.Core.DomainServiceRegister
{
    public class ApplicationService : ZServiceBase, IApplicationService, ILogEnabled
    {
        public ApplicationService(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
