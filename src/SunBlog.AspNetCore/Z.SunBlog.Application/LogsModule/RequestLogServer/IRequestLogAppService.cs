using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.LogsModule.RequestLogServer.Dto;

namespace Z.SunBlog.Application.LogsModule.RequestLogServer
{
    /// <summary>
    /// 异常日志接口
    /// </summary>
    public interface IRequestLogAppService : IApplicationService
    {
        Task<PageResult<RequestLogOutput>> GetPage([FromBody] RequestLogQueryInput input);
    }
}
