using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Entities.Users;

namespace Z.SunBlog.Core.UserModule.DomainManager
{
    public interface IUserDomainManager : IBasicDomainService<ZUserInfo, string>
    {
    }
}
