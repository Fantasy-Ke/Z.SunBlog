using Microsoft.AspNetCore.Mvc;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.AlbumsModule.BlogClient.Dto;

namespace Z.SunBlog.Application.AlbumsModule.BlogClient
{
    /// <summary>
    /// 文章管理
    /// </summary>
    public interface IAlbumsCAppService : IApplicationService
    {
        Task<PageResult<AlbumsOutput>> GetList([FromBody] Pagination dto);

        Task<PageResult<PictureOutput>> Pictures([FromQuery] PicturesQueryInput dto);
    }
}
