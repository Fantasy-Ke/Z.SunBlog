using Microsoft.AspNetCore.Mvc;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.LogsModule.ExceptionlogServer.Dto;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.LogsModule.ExceptionlogServer
{
    /// <summary>
    /// 异常日志接口
    /// </summary>
    public interface IExceptionlogAppService : IApplicationService
    {
        Task<PageResult<ExceptionlogOutput>> GetPage([FromBody] ExceptionlogQueryInput input);

        Task DeleteAsync(KeyDto dto);
    }
}
