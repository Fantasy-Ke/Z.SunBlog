using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.Authorization.Dtos;

public class ZFantasyToken
{
    /// <summary>
    /// 访问令牌
    /// </summary>
    public string AccessToken { get; set; }
    /// <summary>
    /// 刷新令牌
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// 重定向地址
    /// </summary>
    public string RedirectUrl { get; set; }

}
