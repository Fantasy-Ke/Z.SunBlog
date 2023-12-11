using Microsoft.AspNetCore.Mvc;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.EntityFrameworkCore.Extensions;
using Z.SunBlog.Application.FriendLinkModule.BlogServer.Dto;
using Z.SunBlog.Core.SharedDto;
using Z.SunBlog.Core.FriendLinkModule.DomainManager;
using Z.SunBlog.Core.FriendLinkModule;
using Z.Fantasy.Core.ResultResponse.Pager;

namespace Z.SunBlog.Application.FriendLinkModule.BlogServer
{
    /// <summary>
    /// 标签管理
    /// </summary>
    public class FriendLinkAppService : ApplicationService, IFriendLinkAppService
    {
        private readonly IFriendLinkManager _friendLinkManager;
        public FriendLinkAppService(
            IServiceProvider serviceProvider, IFriendLinkManager friendLinkManager) : base(serviceProvider)
        {
            _friendLinkManager = friendLinkManager;
        }

        /// <summary>
        /// 添加修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task CreateOrUpdate(CreateOrUpdateFriendInput dto)
        {
            if (dto.Id != null && dto.Id != Guid.Empty)
            {
                await Update(dto);
                return;
            }

            await Create(dto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteAsync(KeyDto dto)
        {
            await _friendLinkManager.DeleteAsync(dto.Id);
        }

        /// <summary>
        /// 标签列表分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<FriendLinkPageOutput>> GetPage([FromBody] FriendLinkPageQueryInput dto)
       {

            var query = await _friendLinkManager.QueryAsNoTracking
            .WhereIf(!string.IsNullOrWhiteSpace(dto.SiteName), x => x.SiteName.Contains(dto.SiteName))
            .OrderBy(x => x.Sort)
            .Select(x => new FriendLinkPageOutput
            {
                Id = x.Id,
                Status = x.Status,
                SiteName = x.SiteName,
                CreatedTime = x.CreationTime,
                IsIgnoreCheck = x.IsIgnoreCheck,
                Link = x.Link,
                Logo = x.Logo,
                Url = x.Url,
                Sort = x.Sort,
                Remark = x.Remark
            }).ToPagedListAsync(dto);

            return query;
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private async Task Create(CreateOrUpdateFriendInput dto)
        {
            var tags = ObjectMapper.Map<FriendLink>(dto);
            tags.Id = Guid.NewGuid();

            await _friendLinkManager.CreateAsync(tags);
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <returns></returns>
        private async Task Update(CreateOrUpdateFriendInput dto)
        {
            var tags = await _friendLinkManager.FindByIdAsync(dto.Id!.Value);

            ObjectMapper.Map(dto, tags);

            await _friendLinkManager.UpdateAsync(tags!);
        }

    }


}
