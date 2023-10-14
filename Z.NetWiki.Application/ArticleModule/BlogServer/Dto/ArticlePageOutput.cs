﻿using Z.NetWiki.Core.ArticleModule;
using Z.NetWiki.Core.Enum;

namespace Z.NetWiki.Application.ArticleModule.BlogServer.Dto;

public class ArticlePageOutput
{
    /// <summary>
    /// 文章ID
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    /// 封面
    /// </summary>
    public string Cover { get; set; }
    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool IsTop { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
    /// <summary>
    /// 创作类型
    /// </summary>
    public CreationType CreationType { get; set; }
    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime PublishTime { get; set; }
    /// <summary>
    /// 浏览次数
    /// </summary>
    public int Views { get; set; }
    /// <summary>
    /// 栏目名称
    /// </summary>
    public string CategoryName { get; set; }
}