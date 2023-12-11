using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Entities.Organizations;
using Z.Fantasy.Core.Entities.Repositories;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.RedisModule;
using Z.Module.DependencyInjection;
using Z.SunBlog.Application.SystemServiceModule.OrganizationService.Dto;
using Z.SunBlog.Core.Const;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.SystemServiceModule.OrganizationService
{
    /// <summary>
    /// 组织架构
    /// </summary>
    public interface IOrganizationSysAppService : IApplicationService, ITransientDependency
    {
        Task<List<SysOrgPageOutput>> GetPage([FromBody] string name);
        Task AddOrg(AddOrgInput dto);

        Task UpdateOrg(UpdateOrgInput dto);

        Task<List<TreeSelectOutput>> TreeSelect();

        Task Delete(string id);
    }
    
    /// <summary>
    /// 组织架构
    /// </summary>
    public class OrganizationSysAppService : ApplicationService, IOrganizationSysAppService
    {
        private readonly IBasicRepository<ZOrganization, string> _orgDomainService;
        private readonly ICacheManager _cacheManager;
        public OrganizationSysAppService(IServiceProvider serviceProvider, IBasicRepository<ZOrganization, string> orgDomainService, ICacheManager cacheManager) : base(serviceProvider)
        {
            _orgDomainService = orgDomainService;
            _cacheManager = cacheManager;
        }

        private async Task GetChildOrg(List<ZOrganization> orgPanentLists)
        {
            foreach (var orgPan in orgPanentLists)
            {
                var orgList = await _orgDomainService.GetQueryAll()
                    .Where(p => p.ParentId == orgPan.Id).ToListAsync();
                if (orgList.Any())
                {
                    orgPan.Children = orgList;
                    await GetChildOrg(orgList);
                }
            }
        }

        /// <summary>
        /// 组织机构列表查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Description("组织机构列表查询")]
        [HttpPost]
        public async Task<List<SysOrgPageOutput>> GetPage([FromBody] string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var list = await _orgDomainService.GetQueryAll().Where(x => x.Name.Contains(name)).ToListAsync();
                return ObjectMapper.Map<List<SysOrgPageOutput>>(list);
            }

            var treePanentList = await _orgDomainService.GetQueryAll().OrderBy(x => x.Sort)
                .Where(p => p.ParentId == null)
                .ToListAsync();

            await GetChildOrg(treePanentList);

            return ObjectMapper.Map<List<SysOrgPageOutput>>(treePanentList);
        }

        /// <summary>
        /// 添加组织机构
        /// </summary>
        /// <returns></returns>
        [Description("添加组织机构")]
        [HttpPost]
        public async Task AddOrg(AddOrgInput dto)
        {
            var organization = ObjectMapper.Map<ZOrganization>(dto);
            await _orgDomainService.InsertAsync(organization);
        }

        /// <summary>
        /// 更新组织机构
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Description("更新组织机构")]
        [HttpPut]
        public async Task UpdateOrg(UpdateOrgInput dto)
        {
            var organization = await _orgDomainService.FindAsync(dto.Id);
            if (organization == null)
            {
                throw new UserFriendlyException("无效参数");
            }

            ObjectMapper.Map(dto, organization);
            await _orgDomainService.UpdateAsync(organization);
        }

        /// <summary>
        /// 获取机构下拉选项
        /// </summary>
        /// <returns></returns>
        [Description("获取机构下拉选项")]
        [HttpGet]
        public async Task<List<TreeSelectOutput>> TreeSelect()
        {
            var treePanentList = await _orgDomainService.GetQueryAll().OrderBy(x => x.Sort)
                .Where(p => p.ParentId == null)
                .ToListAsync();

            await GetChildOrg(treePanentList);

            return ObjectMapper.Map<List<TreeSelectOutput>>(treePanentList);
        }

        /// <summary>
        /// 删除菜单/按钮
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("删除菜单/按钮"), HttpDelete]
        public async Task Delete(string id)
        {
            await _orgDomainService.DeleteAsync(p=>p.Id == id);
            await _cacheManager.RemoveByPrefixAsync(CacheConst.PermissionKey);
        }
    }

}
