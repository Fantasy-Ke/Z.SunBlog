using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Auditing;

namespace Z.SunBlog.Core.CommentsModule;
/// <summary>
/// 评论表
/// </summary>
public class Comments : FullAuditedEntity<Guid>
{
    /// <summary>
    ///  对应模块ID（null表留言，0代表友链的评论）
    /// </summary>
    public Guid? ModuleId { get; set; }

    /// <summary>
    /// 顶级楼层评论ID
    /// </summary>
    public Guid? RootId { get; set; }

    /// <summary>
    /// 被回复的评论ID
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 当前评论人ID
    /// </summary>
    public string AccountId { get; set; }

    /// <summary>
    /// 回复人ID
    /// </summary>
    public string? ReplyAccountId { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    [MaxLength(1024)]
    public string Content { get; set; }

    /// <summary>
    /// Ip地址
    /// </summary>
    [MaxLength(128)]
    public string IP { get; set; }

    /// <summary>
    /// IP所属地
    /// </summary>
    [MaxLength(128)]
    public string Geolocation { get; set; }
}