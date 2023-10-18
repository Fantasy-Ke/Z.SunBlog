﻿using Microsoft.AspNetCore.Mvc;
using Z.Ddd.Common.DomainServiceRegister;
using Z.Ddd.Common.ResultResponse;
using Z.EntityFrameworkCore.Extensions;
using Z.SunBlog.Core.CustomConfigModule.DomainManager;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using Z.SunBlog.Application.Dto;
using Z.SunBlog.Core.CustomConfigModule;
using Z.Ddd.Common.RedisModule;
using Z.SunBlog.Core.Const;

namespace Z.SunBlog.Application.ConfigModule
{
    public interface ICustomConfigItemAppService : IApplicationService
    {
        Task<PageResult<object>> GetPage([FromBody] CustomConfigItemQueryInput dto);
        Task AddItem(AddCustomConfigItemInput dto);
        Task UpdateItem(UpdateCustomConfigItemInput dto);
    }
    /// <summary>
    /// 标签管理
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
        public async Task<PageResult<object>> GetPage([FromBody] CustomConfigItemQueryInput dto)
        {
            var result = await _customConfigItemManager.QueryAsNoTracking.Where(x => x.ConfigId == dto.Id)
                .Select(x => new { x.Id, x.Json, x.Status, x.CreationTime }).ToPagedListAsync(dto);
            var list = result.Rows.Select(x =>
            {
                var o = JObject.Parse(x.Json);
                o["__Id"] = x.Id;
                o["__Status"] = (int)x.Status;
                o["__CreatedTime"] = x.CreationTime;
                return o;
            }).ToList();
            return new PageResult<object>()
            {
                //Rows = list,
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
            await _customConfigItemManager.Create(item);
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

            await _customConfigItemManager.Create(item);
            await ClearCache();
        }

        internal Task ClearCache()
        {
            return _cacheManager.RefreshCacheAsync(CacheConst.ConfigCacheKey);
        }
    }


}
