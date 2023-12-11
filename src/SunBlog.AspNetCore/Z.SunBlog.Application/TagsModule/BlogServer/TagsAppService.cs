using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.EntityFrameworkCore.Extensions;
using Z.SunBlog.Application.TagsModule.BlogServer.Dto;
using Z.SunBlog.Core.SharedDto;
using Z.SunBlog.Core.TagModule;
using Z.Fantasy.Core.Entities.Enum;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Core.TagModule.DomainManager;

namespace Z.SunBlog.Application.TagsModule.BlogServer
{
    /// <summary>
    /// 标签管理
    /// </summary>
    public class TagsAppService : ApplicationService, ITagsAppService
    {
        private readonly ITagsManager _tagsManager;
        public TagsAppService(
            IServiceProvider serviceProvider, ITagsManager tagsManager) : base(serviceProvider)
        {
            _tagsManager = tagsManager;
        }

        /// <summary>
        /// 添加修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task CreateOrUpdate(CreateOrUpdateTagInput dto)
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
            await _tagsManager.DeleteAsync(dto.Id);
        }


        /// <summary>
        /// 文章标签下拉选项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<SelectOutput>> Select()
        {
            return await _tagsManager.QueryAsNoTracking.Where(x => x.Status == AvailabilityStatus.Enable)
                  .OrderBy(x => x.Sort)
                  .Select(x => new SelectOutput()
                  {
                      Value = x.Id,
                      Label = x.Name
                  })
                  .ToListAsync();
        }

        /// <summary>
        /// 标签列表分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<TagsPageOutput>> GetPage([FromBody] TagsPageQueryInput dto)
       {

            var query = await _tagsManager.QueryAsNoTracking
            .WhereIf(!string.IsNullOrWhiteSpace(dto.Name), x => x.Name.Contains(dto.Name))
            .OrderBy(x => x.Sort)
            .OrderByDescending(x => x.Id)
            .Select(x => new TagsPageOutput
            {
                Id = x.Id,
                Name = x.Name,
                Status = x.Status,
                Sort = x.Sort,
                Cover = x.Cover,
                CreatedTime = x.CreationTime,
                Color = x.Color
            }).ToPagedListAsync(dto);

            return query;
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private async Task Create(CreateOrUpdateTagInput dto)
        {
            var tags = ObjectMapper.Map<Tags>(dto);
            tags.Id = Guid.NewGuid();

            await _tagsManager.CreateAsync(tags);
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <returns></returns>
        private async Task Update(CreateOrUpdateTagInput dto)
        {
            var tags = await _tagsManager.FindByIdAsync(dto.Id!.Value);

            ObjectMapper.Map(dto, tags);

            await _tagsManager.UpdateAsync(tags!);
        }
    }


}
