using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Application.TagsModule.BlogServer.Dto;

public class CreateOrUpdateTagInput// : AddTagInput
{
    /// <summary>
    /// 文章ID
    /// </summary>
    public Guid? Id { get; set; }


    /// <summary>
    /// 标签名称
    /// </summary>
    [Required(ErrorMessage = "标签名称为必填项")]
    [MaxLength(32, ErrorMessage = "标签名限制32个字符内")]
    public string? Name { get; set; }

    /// <summary>
    /// 封面图
    /// </summary>
    [MaxLength(256)]
    [Required(ErrorMessage = "请上传封面图")]
    public string? Cover { get; set; }

    /// <summary>
    /// 标签颜色
    /// </summary>
    [MaxLength(64, ErrorMessage = "标签颜色超过最大长度")]
    public string? Color { get; set; }

    /// <summary>
    /// 标签图标
    /// </summary>
    [MaxLength(32, ErrorMessage = "标签图标超过最大长度")]
    public string? Icon { get; set; }

    /// <summary>
    /// 可用状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }

    /// <summary>
    /// 排序值（值越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序值为必填项")]
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(200,ErrorMessage ="备注超过最大长度")]
    public string? Remark { get; set; }
}