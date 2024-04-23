﻿using System.ComponentModel;
using Z.Foundation.Core.Attributes;

namespace Z.Fantasy.Core.Entities.Enum;

public enum FileType
{
    /// <summary>
    /// 图片
    /// </summary>
    [Description("图片")]
    [EnumName("图片")]
    Image,
    /// <summary>
    /// 视频
    /// </summary>
    [Description("视频")]
    [EnumName("视频")]
    Video,
    /// <summary>
    /// 文件
    /// </summary>
    [Description("文件")]
    [EnumName("文件")]
    File
}
