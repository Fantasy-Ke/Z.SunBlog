using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.EntityFrameworkCore.Extensions;
using Z.SunBlog.Application.AlbumsModule.BlogServer.Dto;
using Z.SunBlog.Core.AlbumsModule;
using Z.SunBlog.Core.AlbumsModule.DomainManager;
using Z.SunBlog.Core.Const;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.AlbumsModule.BlogServer
{
    /// <summary>
    /// AlbumsSAppService相册后台管理
    /// </summary>
    public class AlbumsSAppService : ApplicationService, IAlbumsSAppService
    {
        private readonly IAlbumsManager _albumsManager;
        public AlbumsSAppService(
            IServiceProvider serviceProvider, IAlbumsManager albumsManager) : base(serviceProvider)
        {
            _albumsManager = albumsManager;
        }

        /// <summary>
        /// 添加修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task CreateOrUpdate(CreateOrUpdateAlbumsInput dto)
        {
            if (dto.Id != null && dto.Id != Guid.Empty)
            {
                await Update(dto);
                return;
            }

            await Create(dto);
        }

        /// <summary>
        /// 相册列表分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<AlbumsPageOutput>> GetPage([FromBody] AlbumsPageQueryInput dto)
        {
            return await _albumsManager.QueryAsNoTracking
            .WhereIf(!string.IsNullOrWhiteSpace(dto.Name), x => x.Name.Contains(dto.Name))
            .WhereIf(dto.Type.HasValue, x => x.Type == dto.Type)
            .OrderBy(x => x.Sort)
            .Select(x => new AlbumsPageOutput
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Status = x.Status,
                IsVisible = x.IsVisible,
                Sort = x.Sort,
                Remark = x.Remark,
                Cover = x.Cover,
                CreatedTime = x.CreationTime
            }).ToPagedListAsync(dto);
        }


        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private async Task Create(CreateOrUpdateAlbumsInput dto)
        {
            var albums = ObjectMapper.Map<Albums>(dto);
            await _albumsManager.CreateAsync(albums);
        }

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <returns></returns>
        private async Task Update(CreateOrUpdateAlbumsInput dto)
        {
            var albums = await _albumsManager.FindByIdAsync(dto.Id!.Value);

            ObjectMapper.Map(dto, albums);

            await _albumsManager.UpdateAsync(albums!);
        }


        /// <summary>
        /// 删除菜单/按钮
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("删除菜单/按钮"), HttpDelete]
        public async Task Delete(KeyDto dto)
        {
            await _albumsManager.DeleteAsync(dto.Id);
        }
    }


}
