using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Z.EntityFrameworkCore.Extensions;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.Entities.Enum;
using Z.Fantasy.Core.Exceptions;
using Z.Fantasy.Core.Extensions;
using Z.Fantasy.Core.RedisModule;
using Z.Fantasy.Core.UserSession;
using Z.SunBlog.Application.MenuModule.Dto;
using Z.SunBlog.Core.Const;
using Z.SunBlog.Core.MenuModule;
using Z.SunBlog.Core.MenuModule.DomainManager;
using Z.SunBlog.Core.MessageModule.DomainManager;
using Z.SunBlog.Core.MessageModule.Dto;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.MenuModule;

/// <summary>
/// 标签管理
/// </summary>
public class MenuAppService : ApplicationService, IMenuAppService
{
    private readonly IMenuManager _menuManager;
    private readonly ICacheManager _cacheManager;
    private readonly IMessageManager _messageManager;
    public MenuAppService(
        IServiceProvider serviceProvider, IMenuManager menuManager, ICacheManager cacheManager, IMessageManager messageManager) : base(serviceProvider)
    {
        _menuManager = menuManager;
        _cacheManager = cacheManager;
        _messageManager = messageManager;
    }

    /// <summary>
    /// 菜单列表查询
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [DisplayName("菜单列表查询")]
    [HttpGet]
    public async Task<List<MenuPageOutput>> GetPage([FromQuery] string? name)
    {
        //if (_userSession.UserId != null)
        //{
        var menuList = await _menuManager.QueryAsNoTracking
            .WhereIf(!string.IsNullOrWhiteSpace(name), x => x.Name.Contains(name))
            .OrderBy(x => x.Sort).ToListAsync();

        var menus = menuList.Where(x => x.ParentId == null).ToList();

        await BuildMenu(menus, menuList);

        return ObjectMapper.Map<List<MenuPageOutput>>(menus);
        //}
        //else
        //{
        //    var userId = _userSession.UserId;
        //    var q2 = _sysMenuRepository.AsQueryable().InnerJoin<ZRoleMenu>((menu, roleMenu) => menu.Id == roleMenu.MenuId)
        //        .InnerJoin<ZRoleInfo>((menu, roleMenu, role) => roleMenu.RoleId == role.Id)
        //        .InnerJoin<ZRoleMenu>((menu, roleMenu, role, userRole) => role.Id == userRole.RoleId)
        //        .Where((menu, roleMenu, role, userRole) => role.Status == AvailabilityStatus.Enable && userRole.UserId == userId);
        //    if (!string.IsNullOrWhiteSpace(name))
        //    {
        //        var list = await q2.Where(menu => menu.Name.Contains(name)).Distinct().OrderBy(menu => menu.Sort).OrderBy(menu => menu.Id).ToListAsync();
        //        return list.Adapt<List<MenuPageOutput>>();
        //    }

        //    var menuIdList = await q2.Select(menu => menu.Id).ToListAsync();
        //    var array = menuIdList.Select(x => x as object).ToArray();
        //    var menus = await _sysMenuRepository.AsQueryable()
        //        .Where(x => x.Status == AvailabilityStatus.Enable)
        //        .OrderBy(x => x.Sort)
        //        .OrderBy(x => x.Id)
        //        .ToTreeAsync(x => x.Children, x => x.ParentId, null, array);
        //    return menus.Adapt<List<MenuPageOutput>>();
        //}
    }

    /// <summary>
    /// 添加菜单/按钮
    /// </summary>
    /// <returns></returns>
    [DisplayName("添加菜单/按钮")]
    [HttpPost]
    public async Task AddMenu(AddSysMenuInput dto)
    {
        var sysMenu = ObjectMapper.Map<Menu>(dto);
        if (sysMenu.Type == MenuType.Button)
        {
            sysMenu.Link = sysMenu.Icon = sysMenu.Component = sysMenu.Path = sysMenu.Redirect = sysMenu.RouteName = null;
        }
        else
        {
            if (await _menuManager.QueryAsNoTracking.AnyAsync(x => x.RouteName.ToLower() == dto.RouteName.ToLower()))
            {
                throw new UserFriendlyException("路由名称已存在");
            }
            sysMenu.Code = null;
        }
        await _menuManager.CreateAsync(sysMenu);
        await _cacheManager.RemoveByPrefixAsync(CacheConst.PermissionKey);
    }

    /// <summary>
    /// 修改菜单/按钮
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [DisplayName("更新菜单/按钮")]
    [HttpPut]
    public async Task UpdateMenu(UpdateSysMenuInput dto)
    {
        var sysMenu = await _menuManager.FindByIdAsync(dto.Id);
        if (sysMenu == null)
        {
            throw new UserFriendlyException("无线参数");
        }

        if (dto.Type != MenuType.Button
            && await _menuManager.QueryAsNoTracking.AnyAsync(x => x.RouteName.ToLower() == dto.RouteName.ToLower()
            && x.Id != dto.Id))
        {
            throw new UserFriendlyException("路由名称已存在");
        }
        //检查菜单父子关系是否存在循环引用
        if (dto.ParentId.HasValue && dto.ParentId != sysMenu.ParentId)
        {
            var list = await GetChildMenu(dto.Id, new List<Menu>());
            if (list.Any(x => x.Id == dto.ParentId))
            {
                throw new UserFriendlyException($"请勿将当前{dto.Type.GetDisplayName()}的父级菜单设置为它的子级");
            }
        }

        ObjectMapper.Map(dto, sysMenu);
        if (sysMenu.Type == MenuType.Button)
        {
            sysMenu.Link = sysMenu.Icon = sysMenu.Component = sysMenu.Path = sysMenu.Redirect = sysMenu.RouteName = null;
        }
        else
        {
            sysMenu.Code = null;
        }

        await _menuManager.UpdateAsync(sysMenu);
        await _cacheManager.RemoveByPrefixAsync(CacheConst.PermissionKey);
    }

    /// <summary>
    /// 根据菜单Id获取系统菜单详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [DisplayName("系统菜单详情")]
    [HttpGet]
    public async Task<MenuDetailOutput> Detail([FromQuery] Guid id)
    {
        return await _menuManager.QueryAsNoTracking.Where(x => x.Id == id)
            .Select(x => new MenuDetailOutput
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId,
                Status = x.Status,
                Code = x.Code,
                Sort = x.Sort,
                Component = x.Component,
                Icon = x.Icon,
                IsFixed = x.IsFixed,
                IsIframe = x.IsIframe,
                IsKeepAlive = x.IsKeepAlive,
                IsVisible = x.IsVisible,
                Link = x.Link,
                Remark = x.Remark,
                Path = x.Path,
                Redirect = x.Redirect,
                RouteName = x.RouteName,
                Type = x.Type
            }).FirstAsync();
    }

    /// <summary>
    /// 菜单下拉树
    /// </summary>
    /// <returns></returns>
    [DisplayName("菜单下拉树")]
    [HttpGet]
    public async Task<List<TreeSelectOutput>> TreeSelect()
    {
        var list = await _menuManager.QueryAsNoTracking
            .OrderBy(x => x.Sort).ToListAsync();
        var parentList = list.Where(p => p.ParentId == null).ToList();
        await BuildMenu(parentList, list);
        return ObjectMapper.Map<List<TreeSelectOutput>>(parentList);
    }

    /// <summary>
    /// 删除菜单/按钮
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [DisplayName("删除菜单/按钮"), HttpDelete]
    public async Task Delete(KeyDto dto)
    {
        await _menuManager.DeleteAsync(dto.Id);
        await _cacheManager.RemoveCacheAsync(CacheConst.PermissionKey);
    }

    /// <summary>
    /// 修改菜单/按钮状态
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [DisplayName("修改菜单/按钮状态"), HttpPut]
    public async Task SetStatus(AvailabilityDto dto)
    {
        var entity = await _menuManager.FindByIdAsync((Guid)dto.GId!);
        entity.Status = dto.Status;
        var entityAfter = await _menuManager.UpdateAsync(entity);
        await _cacheManager.RemoveCacheAsync(CacheConst.PermissionKey);
    }

    /// <summary>
    /// 获取当前登录用户可用菜单
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取当前登录用户可用菜单")]
    [HttpGet]
    public async Task<List<RouterOutput>> PermissionMenus()
    {
        var userId = UserService.UserId;

        var value = await _cacheManager.GetCacheAsync($"{CacheConst.PermissionMenuKey}{userId}", async () =>
        {
            var queryList = await _menuManager.QueryAsNoTracking
                .Where(x => x.Status == AvailabilityStatus.Enable)
                .OrderBy(x => x.Sort).ToListAsync();
            //if (_authManager.IsSuperAdmin)
            //{

            var menus = queryList.Where(x => x.ParentId == null && x.Type != MenuType.Button).ToList();

            await BuildMenu(menus, queryList.Where(x => x.Type != MenuType.Button).ToList());
            //}
            //else
            //{
            //    List<long> menuIdList = await _sysMenuRepository.AsSugarClient().Queryable<SysRole>().InnerJoin<SysUserRole>((role, userRole) => userRole.RoleId == role.Id)
            //        .InnerJoin<SysRoleMenu>((role, userRole, roleMenu) => role.Id == roleMenu.RoleId)
            //        .InnerJoin<SysMenu>((role, userRole, roleMenu, menu) => roleMenu.MenuId == menu.Id)
            //        .Where((role, userRole, roleMenu, menu) => role.Status == AvailabilityStatus.Enable && userRole.UserId == userId && menu.Status == AvailabilityStatus.Enable)
            //        .Select((role, userRole, roleMenu) => roleMenu.MenuId).ToListAsync();
            //    var array = menuIdList.Select(x => x as object).ToArray();
            //    list = await queryable
            //    .ToTreeAsync(x => x.Children, x => x.ParentId, null, array);
            //    RemoveButton(list);
            //}
            return menus != null && menus.Count > 0 ? ObjectMapper.Map<List<RouterOutput>>(menus) : null;
        }, TimeSpan.FromDays(1));
        return value ?? new List<RouterOutput>();
    }

    /// <summary>
    /// 菜单按钮树
    /// </summary>
    /// <returns></returns>
    [DisplayName("菜单按钮树")]
    [HttpGet]
    public async Task<List<TreeSelectOutput>> TreeMenuButton()
    {
        //long userId = _authManager.UserId;
        //List<SysMenu> menus;
        //if (_authManager.IsSuperAdmin)//超级管理员
        //{
        var menusList = await _menuManager.QueryAsNoTracking.ToListAsync();
        var parentList = menusList.Where(p => p.ParentId == null).ToList();
        await BuildMenu(parentList, menusList);
        //}
        //else
        //{
        //    menus = await _sysMenuRepository.AsQueryable()
        //        .InnerJoin<SysRoleMenu>((menu, roleMenu) => menu.Id == roleMenu.MenuId)
        //        .InnerJoin<SysRole>((menu, roleMenu, role) => roleMenu.RoleId == role.Id)
        //        .InnerJoin<SysUserRole>((menu, roleMenu, role, userRole) => role.Id == userRole.RoleId)
        //        .Where((menu, roleMenu, role, userRole) => menu.Status == AvailabilityStatus.Enable &&
        //                                                   role.Status == AvailabilityStatus.Enable &&
        //                                                   userRole.UserId == userId)
        //        .Select(menu => menu)
        //        .ToTreeAsync(x => x.Children, x => x.ParentId, null);
        //}

        return ObjectMapper.Map<List<TreeSelectOutput>>(parentList);
    }

    /// <summary>
    /// 校验权限
    /// </summary>
    /// <param name="code">权限标识</param>
    /// <returns></returns>
    [NonAction]
    public async Task<bool> CheckPermission(string code)
    {
        //if (_authManager.IsSuperAdmin) 
        return true;
        //var cache = await GetAuthButtonCodeList(_authManager.UserId);
        //var output = cache.FirstOrDefault(x => x.Code.Contains(code, StringComparison.CurrentCultureIgnoreCase));
        //return output?.Access ?? true;
    }

    /// <summary>
    /// 获取指定用户的访问权限集合
    /// </summary>
    /// <param name="userId">系统用户id</param>
    /// <returns></returns>
    //[NonAction]
    //public async Task<List<CheckPermissionOutput>> GetAuthButtonCodeList(long userId)
    //{
    //    var cache = await _cacheManager.GetCacheAsync($"{CacheConst.PermissionButtonCodeKey}{userId}", async () =>
    //    {
    //        var queryable = _sysMenuRepository.AsSugarClient().Queryable<SysRole>()
    //            .InnerJoin<SysUserRole>((role, userRole) => role.Id == userRole.RoleId)
    //            .InnerJoin<SysRoleMenu>((role, userRole, roleMenu) => role.Id == roleMenu.RoleId)
    //            .Where(role => role.Status == AvailabilityStatus.Enable)
    //            .Select((role, userRole, roleMenu) => roleMenu);
    //        var list = await _sysMenuRepository.AsQueryable().LeftJoin(queryable, (menu, roleMenu) => menu.Id == roleMenu.MenuId)
    //               .Where(menu => menu.Type == MenuType.Button)
    //               .Select((menu, roleMenu) => new CheckPermissionOutput
    //               {
    //                   Code = menu.Code,
    //                   Access = SqlFunc.IIF(SqlFunc.IsNull(roleMenu.Id, 0) > 0 || menu.Status == AvailabilityStatus.Disable, true, false)
    //               }).ToListAsync();
    //        return list.Distinct().ToList();
    //    }, TimeSpan.FromDays(1));
    //    return cache.Value;
    //}

    /// <summary>
    /// 移除菜单中的按钮
    /// </summary>
    /// <param name="menus"></param>
    public void RemoveButton(List<Menu> menus)
    {
        for (int i = menus.Count - 1; i >= 0; i--)
        {
            if (menus[i].Type == MenuType.Button)
            {
                menus.Remove(menus[i]);
                continue;
            }
            if (menus[i].Children.Any())
            {
                RemoveButton(menus[i].Children);
            }
        }
    }


    #region 私有方法

    private async Task BuildMenu(List<Menu> menuPanentLists, List<Menu> menusAllList)
    {
        foreach (var menuPan in menuPanentLists)
        {
            var menuList = menusAllList
                .Where(p => p.ParentId == menuPan.Id).ToList();
            if (menuList.Any())
            {
                menuPan.Children = menuList;
                await BuildMenu(menuList, menusAllList);
            }
        }
    }

    private async Task<List<Menu>> GetChildMenu(Guid id, List<Menu> orgLists)
    {
        var orgList = _menuManager.QueryAsNoTracking.Where(p => p.ParentId == id);
        if (await orgList.AnyAsync())
        {
            orgLists.AddRange(orgList);
            foreach (var org in orgLists)
            {
                return await GetChildMenu(org.Id, orgLists);
            }
        }
        return orgLists;
    }

    #endregion
}
