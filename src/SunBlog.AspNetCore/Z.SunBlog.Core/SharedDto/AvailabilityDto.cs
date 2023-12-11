using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Core.SharedDto;

public class AvailabilityDto
{

    /// <summary>
    /// 主键
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 主键
    /// </summary>
    public Guid? GId { get; set; }
    /// <summary>
    /// 状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }
}