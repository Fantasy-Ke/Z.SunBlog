using Microsoft.AspNetCore.Mvc;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.LogsModule.RequestLogServer.Dto;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.LogsModule.RequestLogServer
{
    /// <summary>
    /// 异常日志接口
    /// </summary>
    public interface IRequestLogAppService : IApplicationService
    {
        Task<PageResult<RequestLogOutput>> GetPage([FromBody] RequestLogQueryInput input);

        Task DeleteAsync(KeyDto dto);
    }
}
