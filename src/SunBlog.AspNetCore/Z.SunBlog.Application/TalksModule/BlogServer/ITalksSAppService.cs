using Microsoft.AspNetCore.Mvc;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.TalksModule.BlogServer.Dto;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.TalksModule.BlogServer
{
    /// <summary>
    /// 相册管理
    /// </summary>
    public interface ITalksSAppService : IApplicationService
    {
        Task<PageResult<TalksPageOutput>> GetPage([FromBody] TalksPageQueryInput dto);

        Task CreateOrUpdate(CreateOrUpdateTalksInput dto);

        Task DeleteAsync(KeyDto dto);

    }
}
