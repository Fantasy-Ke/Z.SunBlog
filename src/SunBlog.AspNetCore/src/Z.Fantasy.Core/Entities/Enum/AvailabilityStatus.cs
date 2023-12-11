using System.ComponentModel;

namespace Z.Fantasy.Core.Entities.Enum;
/// <summary>
/// 可用状态
/// </summary>
public enum AvailabilityStatus
{
    /// <summary>
    /// 启用
    /// </summary>
    [Description("启用")]
    Enable,

    /// <summary>
    /// 禁用
    /// </summary>
    [Description("禁用")]
    Disable
}