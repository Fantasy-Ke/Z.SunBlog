﻿using Z.Foundation.Core.Entities.Enum;

namespace Z.SunBlog.Application.MenuModule.Dto;

public class MenuPageOutput
{
    /// <summary>
    ///  菜单Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 菜单图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 菜单类型
    /// </summary>
    public MenuType Type { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    public string? Component { get; set; }

    /// <summary>
    /// 可用状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }

    /// <summary>
    /// 排序值（值越小越靠前）
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 子菜单
    /// </summary>
    public List<MenuPageOutput> Children { get; set; } = new();
}