
using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Core.ArticleModule;
/// <summary>
/// 文章表
/// </summary>
public class Article : FullAuditedEntity<Guid>
{
    /// <summary>
    /// 标题
    /// </summary>
    [MaxLength(128)]
    public string Title { get; set; }

    /// <summary>
    /// 概要
    /// </summary>
    [MaxLength(256)]
    public string Summary { get; set; }

    /// <summary>
    /// 封面图
    /// </summary>
    [MaxLength(256)]
    public string Cover { get; set; }

    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool IsTop { get; set; }

    /// <summary>
    /// 浏览量
    /// </summary>
    public int Views { get; set; }

    /// <summary>
    /// 作者
    /// </summary>
    [MaxLength(32)]
    public string Author { get; set; }

    /// <summary>
    /// 原文地址
    /// </summary>
    [MaxLength(256)]
    public string? Link { get; set; }

    /// <summary>
    /// 创作类型
    /// </summary>
    public CreationType CreationType { get; set; }

    /// <summary>
    /// 文章正文（Html或markdown）
    /// </summary>
    [MaxLength(int.MaxValue)]
    public string Content { get; set; }

    /// <summary>
    /// 文章正文是否为html代码
    /// </summary>
    public bool IsHtml { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime PublishTime { get; set; }

    /// <summary>
    /// 可用状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }

    /// <summary>
    /// 排序值（值越小越靠前）
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否允许评论
    /// </summary>
    public bool IsAllowComments { get; set; }

    /// <summary>
    /// 过期时间（过期后文章不显示）
    /// </summary>
    public DateTime? ExpiredTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedTime { get; set; }
}