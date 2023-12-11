using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Auditing;

namespace Z.SunBlog.Core.PicturesModule;

/// <summary>
/// 相册图片表
/// </summary>
public class Pictures : FullAuditedEntity<Guid>
{
    /// <summary>
    /// 相册Id
    /// </summary>
    public Guid AlbumId { get; set; }

    /// <summary>
    /// 图片地址
    /// </summary>
    [MaxLength(256)]
    public string Url { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(256)]
    public string? Remark { get; set; }
}