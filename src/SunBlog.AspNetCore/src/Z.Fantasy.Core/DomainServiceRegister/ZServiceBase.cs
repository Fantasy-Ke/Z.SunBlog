using AutoMapper;
using AutoMapper.Internal.Mappers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.AutoMapper;
using Z.Fantasy.Core.UserSession;

namespace Z.Fantasy.Core.DomainServiceRegister;

public abstract class ZServiceBase
{
    /// <summary>
    /// Reference to the object to object mapper.
    /// </summary>
    public IMapper ObjectMapper { get; set; }

    /// <summary>
    /// 用户信息
    /// </summary>
    public IUserSession UserService { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public ZServiceBase(IServiceProvider serviceProvider)
    {
        ObjectMapper = serviceProvider.GetRequiredService<IMapper>();
        UserService = serviceProvider.GetRequiredService<IUserSession>();
    }
}
