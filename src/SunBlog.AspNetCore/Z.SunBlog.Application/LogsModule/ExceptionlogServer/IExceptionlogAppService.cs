using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.LogsModule.ExceptionlogServer.Dto;

namespace Z.SunBlog.Application.LogsModule.ExceptionlogServer
{
    /// <summary>
    /// 异常日志接口
    /// </summary>
    public interface IExceptionlogAppService : IApplicationService
    {
        Task<PageResult<ExceptionlogOutput>> GetPage([FromBody] ExceptionlogQueryInput input);
    }
}
