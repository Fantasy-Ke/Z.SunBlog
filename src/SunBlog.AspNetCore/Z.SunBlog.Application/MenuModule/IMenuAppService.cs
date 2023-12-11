using Microsoft.AspNetCore.Mvc;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.SunBlog.Application.MenuModule.Dto;
using Z.SunBlog.Core.MenuModule;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.MenuModule
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public interface IMenuAppService : IApplicationService
    {
        Task<List<MenuPageOutput>> GetPage([FromQuery] string? name);

        Task AddMenu(AddSysMenuInput dto);

        Task UpdateMenu(UpdateSysMenuInput dto);

        Task<MenuDetailOutput> Detail([FromQuery] Guid id);

        Task<List<TreeSelectOutput>> TreeSelect();

        Task Delete(KeyDto dto);

        Task SetStatus(AvailabilityDto dto);

        Task<List<RouterOutput>> PermissionMenus();

        Task<List<TreeSelectOutput>> TreeMenuButton();

        void RemoveButton(List<Menu> menus);

    }
}
