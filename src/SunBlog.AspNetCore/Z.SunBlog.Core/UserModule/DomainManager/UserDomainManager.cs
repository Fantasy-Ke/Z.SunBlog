using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Entities.Users;
using Z.Fantasy.Core.Exceptions;

namespace Z.SunBlog.Core.UserModule.DomainManager
{
    public class UserDomainManager : BasicDomainService<ZUserInfo, string>, IUserDomainManager
    {
        public UserDomainManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task ValidateOnCreateOrUpdate(ZUserInfo entity)
        {
            var count = await Query
                .Where(a => a.UserName == entity.UserName && a.Id != entity.Id).CountAsync();

            if (count > 0)
            {
                this.ThrowRepetError(entity.UserName);
            }
        }
    }
}
