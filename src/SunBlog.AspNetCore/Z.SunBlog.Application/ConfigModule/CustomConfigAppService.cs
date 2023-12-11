using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.ComponentModel;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.SunBlog.Core.Const;
using Z.SunBlog.Core.CustomConfigModule;
using System.Collections;
using Z.Fantasy.Core.RedisModule;
using Z.SunBlog.Core.CustomConfigModule.DomainManager;
using Microsoft.EntityFrameworkCore;
using Z.EntityFrameworkCore.Extensions;
using Z.Fantasy.Core.Exceptions;
using Z.SunBlog.Core.SharedDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
using Z.SunBlog.Application.ConfigModule.Dto;
using Z.Fantasy.Core.ResultResponse.Pager;

namespace Z.SunBlog.Application.ConfigModule
{
    public interface ICustomConfigAppService : IApplicationService
    {
        Task<T> Get<T>();

        Task<dynamic> GetConfig([FromQuery] string code);

        Task<PageResult<CustomConfigPageOutput>> GetPage(CustomConfigQueryInput dto);

        Task AddConfig(AddCustomConfigInput dto);

        Task UpdateConfig(UpdateCustomConfigInput dto);

        Task<CustomConfigDetailOutput> GetFormJson(GetConfigDetailInput input);

        Task SetJson(CustomConfigSetJsonInput dto);

        Task Generate(KeyDto dto);

        Task DeleteClass(KeyDto dto);

        Task Delete(KeyDto dto);
    }
    /// <summary>
    /// CustomConfigAppService文章管理
    /// </summary>
    public class CustomConfigAppService : ApplicationService, ICustomConfigAppService
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICustomConfigManager _customConfigManager;
        private readonly ICustomConfigItemManager _customConfigItemManager;
        private readonly IHostEnvironment _environment;
        public CustomConfigAppService(IServiceProvider serviceProvider, 
            ICacheManager cacheManager, 
            ICustomConfigManager customConfigManager, 
            ICustomConfigItemManager customConfigItemManager, 
            IHostEnvironment environment) : base(serviceProvider)
        {
            _cacheManager = cacheManager;
            _customConfigManager = customConfigManager;
            _customConfigItemManager = customConfigItemManager;
            _environment = environment;
        }

        /// <summary>
        /// 获取强类型配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [NonAction]
        public async Task<T> Get<T>()
        {
            var type = typeof(T);
            bool isList = typeof(IEnumerable).IsAssignableFrom(type);
            string code = type.IsGenericType && isList ? type.GenericTypeArguments[0].Name : type.Name;
            var value = await _cacheManager.GetCacheAsync($"{CacheConst.ConfigCacheKey}{code}", async () =>
            {
                var queryable = _customConfigManager.QueryAsNoTracking
                .Join(_customConfigItemManager.QueryAsNoTracking, c => c.Id, item => item.ConfigId, (c, item) => new { config = c, item = item })
                    .Where(c => c.config.Code == code)
                    .Select(c => c.item.Json);
                string json;
                if (isList)
                {
                    List<string> list = await queryable.ToListAsync();
                    if (!list.Any()) return default(T);
                    json = $"[{string.Join(",", list)}]";
                }
                else
                {
                    json = await queryable.FirstOrDefaultAsync();
                }

                return string.IsNullOrWhiteSpace(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
            }, TimeSpan.FromDays(1));

            return value;
        }

        /// <summary>
        /// 获取自定义配置
        /// </summary>
        /// <param name="code">自定义配置唯一编码</param>
        /// <returns></returns>
        [DisplayName("获取自定义配置")]
        [HttpGet]
        public async Task<dynamic> GetConfig([FromQuery] string code)
        {
            var value = await _cacheManager.GetCacheAsync<object>($"{CacheConst.ConfigCacheKey}{code}", async () =>
            {
                var c = await _customConfigManager.QueryAsNoTracking.FirstOrDefaultAsync(x => x.Code == code);
                if (c == null) return null;
                var queryable = _customConfigManager.QueryAsNoTracking
                .Join(_customConfigItemManager.QueryAsNoTracking, p => p.Id,
                c => c.ConfigId,
                (c, p) => new { config = c, item = p })
                    .Where(all => all.config.Code == code)
                    .Select(all => all.item.Json);
                if (c.IsMultiple)
                {
                    List<string> list = await queryable.ToListAsync();
                    if (!list.Any()) return null;
                    return JArray.Parse($"[{string.Join(",", list)}]");
                }

                string s = await queryable.FirstAsync();
                return string.IsNullOrWhiteSpace(s) ? null : JObject.Parse(s);
            }, TimeSpan.FromDays(1));
            return value;
        }

        /// <summary>
        /// 自定义配置分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("自定义配置分页查询")]
        [HttpPost]
        public async Task<PageResult<CustomConfigPageOutput>> GetPage([FromBody] CustomConfigQueryInput dto)
        {
            var dataList = await _customConfigManager.QueryAsNoTracking
                .WhereIf(!string.IsNullOrWhiteSpace(dto.Name), x => x.Name.Contains(dto.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(dto.Code), x => x.Code.Contains(dto.Code))
                .OrderByDescending(x => x.Id)
                .Select(x => new CustomConfigPageOutput
                {
                    Id = x.Id,
                    Status = x.Status,
                    Remark = x.Remark,
                    Name = x.Name,
                    Code = x.Code,
                    IsMultiple = x.IsMultiple,
                    AllowCreationEntity = x.AllowCreationEntity,
                    IsGenerate = x.IsGenerate,
                    CreatedTime = x.CreationTime
                }).ToPagedListAsync(dto);

            var configId = dataList.Rows.Select(x => x.Id).ToList();
            var configItem = await _customConfigItemManager.QueryAsNoTracking.Where(p => configId.Contains(p.ConfigId)).ToListAsync();
            dataList.Rows.ToList().ForEach(x => x.ConfigItemId = configItem.Where(p => p.ConfigId == x.Id).Select(p=>p.Id).ToList());

            return dataList;
        }

        /// <summary>
        /// 添加自定义配置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("添加自定义配置")]
        [HttpPost]
        public async Task AddConfig(AddCustomConfigInput dto)
        {
            if (await _customConfigManager.QueryAsNoTracking.AnyAsync(x => x.Code == dto.Code))
            {
                throw new UserFriendlyException("编码已存在");
            }
            var config = ObjectMapper.Map<CustomConfig>(dto);
            await _customConfigManager.CreateAsync(config);
            await _cacheManager.RemoveByPrefixAsync($"{CacheConst.ConfigCacheKey}{config.Code}");
        }

        /// <summary>
        /// 修改自定义配置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("修改自定义配置")]
        [HttpPost]
        public async Task UpdateConfig(UpdateCustomConfigInput dto)
        {
            var config = await _customConfigManager.FindByIdAsync(dto.Id);
            if (config == null)
            {
                throw new UserFriendlyException("无效参数");
            }
            if (dto.Code != config.Code && await _customConfigManager.IsAnyAsync(x => x.Code == dto.Code && x.Id != dto.Id))
            {
                throw new UserFriendlyException("编码已存在");
            }
            ObjectMapper.Map(dto, config);
            await _customConfigManager.UpdateAsync(config);
            await _cacheManager.RemoveByPrefixAsync($"{CacheConst.ConfigCacheKey}{config.Code}");
        }

        /// <summary>
        /// 获取配置表单设计和表单数据
        /// </summary>
        [DisplayName("获取配置表单设计和表单数据")]
        [HttpPost]
        public async Task<CustomConfigDetailOutput> GetFormJson(GetConfigDetailInput input)
        {
            var output = new CustomConfigDetailOutput()
            {
                ItemId = input.ItemId ?? null
            };
            string? json = await _customConfigManager.QueryAsNoTracking
                     .Where(x => x.Id == input.Id)
                     .Select(x => x.Json).FirstOrDefaultAsync();
            if (string.IsNullOrWhiteSpace(json))
            {
                return output;
            };
            output.FormJson = json;
            if (input.ItemId == null || !input.ItemId.HasValue) return output;
            var data = input.ItemId.HasValue ? await _customConfigItemManager.QueryAsNoTracking.Where(x => x.Id == input.ItemId)
                .Select(x => new { x.Id, x.Json }).FirstAsync()
                : await _customConfigItemManager.QueryAsNoTracking
                .Where(x => x.ConfigId == input.Id).Select(x => new { x.Id, x.Json })
                .FirstAsync();
            if (data != null)
            {
                output.DataJson = data.Json;
                output.ItemId = data.Id;
            }
            return output;
        }

        /// <summary>
        /// 修改配置表单设计
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("修改配置表单设计")]
        [HttpPatch]
        public async Task SetJson(CustomConfigSetJsonInput dto)
        {
            var entity = await _customConfigManager.QueryAsNoTracking.Where(x => x.Id == dto.Id).FirstAsync();
            entity.Json = dto.Json;
            await _customConfigManager.UpdateAsync(entity);
            await ClearCache();
        }

        internal Task ClearCache()
        {
            return _cacheManager.RemoveByPrefixAsync(CacheConst.ConfigCacheKey);
        }

        /// <summary>
        /// 生成自定配置类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("生成自定配置类")]
        [HttpPost]
        public async Task Generate(KeyDto dto)
        {
            if (!_environment.IsDevelopment())
            {
                throw new UserFriendlyException("生成配置类仅限开发环境使用");
            }
            var config = await _customConfigManager.FindByIdAsync(dto.Id);
            if (config == null) throw new UserFriendlyException("无效参数");
            if (string.IsNullOrWhiteSpace(config.Json)) throw new UserFriendlyException("请配置设计");
            var controls = ResolveJson(config.Json);
            if (!controls.Any()) throw new UserFriendlyException("请配置设计");
            //await GenerateCode(config.Code, controls);
            await _customConfigManager.UpdateAsync(new CustomConfig()
            {
                IsGenerate = true
            }, x => x.Id == config.Id);
            await _cacheManager.RemoveByPrefixAsync($"{CacheConst.ConfigCacheKey}{config.Code}");
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
            await _customConfigManager.DeleteAsync(x => x.Id == dto.Id);
            await ClearCache();
        }

        /// <summary>
        /// 删除自定义配置类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        [DisplayName("删除自定义配置类")]
        [HttpPatch]
        public async Task DeleteClass(KeyDto dto)
        {
            if (!_environment.IsDevelopment())
            {
                throw new UserFriendlyException("删除配置类仅限开发环境使用");
            }

            string className = await _customConfigManager.QueryAsNoTracking.Where(x => x.Id == dto.Id).Select(x => x.Code).FirstAsync();
            if (className == null) throw new UserFriendlyException("无效参数");
            //string path = Path.Combine(_environment.ContentRootPath.Replace(_environment.ApplicationName, ""), "Easy.Admin.Core/Config", $"{className}.cs");
            //if (System.IO.File.Exists(path))
            //{
            //    System.IO.File.DeleteAsync(path);
            //}
            await _customConfigManager.UpdateAsync(new CustomConfig() { IsGenerate = false }, x => x.Id == dto.Id);
            await _cacheManager.RemoveByPrefixAsync($"{CacheConst.ConfigCacheKey}{className}");
        }

        /// <summary>
        /// 解析表单设计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [NonAction]
        public List<CustomControl> ResolveJson(string json)
        {
            string s = "{\"key\":\\d+,\"type\":\"(input|select|date|switch|number|textarea|radio|checkbox|time|time-range|date-range|rate|color|slider|cascader|rich-editor|file-upload|picture-upload)\".*?\"id\".*?}";
            string value = string.Join(",", Regex.Matches(json, s, RegexOptions.IgnoreCase).Select(x => x.Value));
            string temp = $"[{value}]";
            return JsonConvert.DeserializeObject<List<CustomControl>>(temp);
        }
    }
}
