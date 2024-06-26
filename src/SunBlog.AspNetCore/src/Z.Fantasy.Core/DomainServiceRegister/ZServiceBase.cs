﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Z.Foundation.Core.UserSession;

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
