using Microsoft.AspNetCore.Mvc;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.EntityFrameworkCore.Extensions;
using Z.SunBlog.Core.CustomConfigModule.DomainManager;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using Z.SunBlog.Core.CustomConfigModule;
using Z.Fantasy.Core.RedisModule;
using Z.SunBlog.Core.Const;
using Z.SunBlog.Core.SharedDto;
using Newtonsoft.Json;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.ConfigModule.Dto;

namespace Z.SunBlog.Application.ConfigModule
{
    public interface ICustomConfigItemAppService : IApplicationService
    {
        Task<PageResult<string>> GetPage([FromBody] CustomConfigItemQueryInput dto);
        Task AddItem(AddCustomConfigItemInput dto);
        Task UpdateItem(UpdateCustomConfigItemInput dto);

        Task Delete(KeyDto dto);
    }
    /// <summary>
    /// CustomConfigItemAppService标签管理
    /// </summary>
    public class CustomConfigItemAppService : ApplicationService, ICustomConfigItemAppService
    {
        private readonly ICustomConfigItemManager _customConfigItemManager;
        private readonly ICacheManager _cacheManager;
        public CustomConfigItemAppService(
            IServiceProvider serviceProvider,
            ICustomConfigItemManager customConfigItemManager,
            ICacheManager cacheManager) : base(serviceProvider)
        {
            _customConfigItemManager = customConfigItemManager;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 自定义配置项分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<string>> GetPage([FromBody] CustomConfigItemQueryInput dto)
        {
            var result = await _customConfigItemManager.QueryAsNoTracking.Where(x => x.ConfigId == dto.Id)
                .Select(x => new { x.Id, x.Json, x.Status, x.CreationTime }).ToPagedListAsync(dto);
            var list = result.Rows.Select(x =>
            {
                var o = JObject.Parse(x.Json);
                o["tempId"] = x.Id;
                o["tempStatus"] = (int)x.Status;
                o["tempCreatedTime"] = x.CreationTime;
                return JsonConvert.SerializeObject(o);
            }).ToList();
            return new PageResult<string>()
            {
                Rows = list,
                PageNo = result.PageNo,
                PageSize = result.PageSize,
                Pages = result.Pages,
                Total = result.Total
            };
        }

        /// <summary>
        /// 添加自定义配置子项
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task AddItem(AddCustomConfigItemInput dto)
        {
            var item = ObjectMapper.Map<CustomConfigItem>(dto);
            await _customConfigItemManager.CreateAsync(item);
            await ClearCache();
        }

        /// <summary>
        /// 修改自定义配置子项
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UpdateItem(UpdateCustomConfigItemInput dto)
        {
            var item = await _customConfigItemManager.FindByIdAsync(dto.Id);

            ObjectMapper.Map(dto, item);

            await _customConfigItemManager.UpdateAsync(item);
            await ClearCache();
        }


        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("删除信息")]
        [HttpDelete]
        public async Task Delete(KeyDto dto)
        {
            await _customConfigItemManager.DeleteAsync(x => x.Id == dto.Id);
            await ClearCache();
        }
        internal Task ClearCache()
        {
            return _cacheManager.RemoveByPrefixAsync(CacheConst.ConfigCacheKey);
        }
    }


}
