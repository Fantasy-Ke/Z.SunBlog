﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Ddd.Common.DomainServiceRegister
{
    public class ApplicationService : ZServiceBase, IApplicationService
    {
        public ApplicationService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
