using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Entities.Enum;
using Z.Fantasy.Core.Extensions;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.EntityFrameworkCore.Extensions;
using Z.SunBlog.Application.AlbumsModule.BlogClient.Dto;
using Z.SunBlog.Core.AlbumsModule.DomainManager;
using Z.SunBlog.Core.PicturesModule.DomainManager;

namespace Z.SunBlog.Application.AlbumsModule.BlogClient
{
    /// <summary>
    /// AlbumsCAppService相册前台管理
    /// </summary>
    public class AlbumsCAppService : ApplicationService, IAlbumsCAppService
    {
        private readonly IAlbumsManager _albumsManager;
        private readonly IPicturesManager _picturesManager;
        public AlbumsCAppService(
            IServiceProvider serviceProvider,
            IAlbumsManager albumsManager,
            IPicturesManager picturesManager) : base(serviceProvider)
        {
            _albumsManager = albumsManager;
            _picturesManager = picturesManager;
        }

        /// <summary>
        /// 相册列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<AlbumsOutput>> GetList([FromBody] Pagination dto)
        {
            return await _albumsManager.QueryAsNoTracking.Where(x => x.IsVisible && x.Status == AvailabilityStatus.Enable)
              .OrderBy(x => x.Sort)
              .Select(x => new AlbumsOutput
              {
                  Id = x.Id,
                  Name = x.Name,
                  Cover = x.Cover,
                  Remark = x.Remark,
                  CreatedTime = x.CreationTime
              }).ToPagedListAsync(dto);
        }

        /// <summary>
        /// 相册下的图片
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PageResult<PictureOutput>> Pictures([FromQuery] PicturesQueryInput dto)
        {
            var album = await _albumsManager.QueryAsNoTracking.Where(x => x.Id == dto.AlbumId && x.IsVisible && x.Status == AvailabilityStatus.Enable).Select(x => new
            {
                x.Name,
                x.Cover
            }).FirstAsync();

            HttpExtension.Fill(album);

            return await _albumsManager.QueryAsNoTracking
                .Where(albums => albums.IsVisible && albums.Status == AvailabilityStatus.Enable && albums.Id == dto.AlbumId)
                .Join(_picturesManager.QueryAsNoTracking,
                    a=>a.Id,
                    p=>p.AlbumId,
                    (a, p) => p
                 )
                 .OrderByDescending(c=>c.Url)
                 .Select(pictures => new PictureOutput { Id = pictures.Id, Url = pictures.Url })
                 .ToPagedListAsync(dto);
        }
    }


}
