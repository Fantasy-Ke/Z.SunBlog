using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Dynamic.Core;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.Fantasy.Core.UserSession;
using Z.EntityFrameworkCore.Extensions;
using Z.SunBlog.Application.PictureModule.BlogServer.Dto;
using Z.SunBlog.Core.AlbumsModule;
using Z.SunBlog.Core.AlbumsModule.DomainManager;
using Z.SunBlog.Core.PicturesModule;
using Z.SunBlog.Core.PicturesModule.DomainManager;
using Z.SunBlog.Core.PraiseModule.DomainManager;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.PictureModule.BlogServer
{
    /// <summary>
    /// 相册图片管理
    /// </summary>
    public class PictureSAppService : ApplicationService, IPictureSAppService
    {
        private readonly IPicturesManager _pictureManager;
        private readonly IAlbumsManager _albumsManager;
        public PictureSAppService(
            IServiceProvider serviceProvider, IPicturesManager pictureManager, IAlbumsManager albumsManager) : base(serviceProvider)
        {
            _pictureManager = pictureManager;
            _albumsManager = albumsManager;
        }

        /// <summary>
        /// 上传图片到相册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("上传图片到相册")]
        [HttpPost]
        public async Task AddPictures(AddPictureInput dto)
        {
            var entity = ObjectMapper.Map<Pictures>(dto);
            await _pictureManager.CreateAsync(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task Delete(KeyDto dto)
        {
            await _pictureManager.DeleteAsync(x => x.Id == dto.Id);
        }

        /// <summary>
        /// 删除上册图片
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("删除相册图片")]
        [HttpPost]
        public async Task<PageResult<PicturesPageOutput>> GetPage([FromBody] PicturesPageQueryInput dto)
        {
            return await _pictureManager.QueryAsNoTracking.GroupJoin(
                _albumsManager.QueryAsNoTracking,p=>p.AlbumId,
                c=>c.Id,(p,c)=>new { pic=p,alb=c })
                .SelectMany(a => a.alb.DefaultIfEmpty(), (m, n) => new
                {
                    alb = n,
                    picAlb = m
                })
            .Where(c => c.picAlb.pic.AlbumId == dto.Id)
            //.WhereIf(_authManager.AuthPlatformType is null or AuthPlatformType.Blog, (pictures, albums) => albums.IsVisible)
            .Select(c => new PicturesPageOutput { Id = c.picAlb.pic.Id, Url = c.picAlb.pic.Url })
            .ToPagedListAsync(dto);
        }
    }


}
