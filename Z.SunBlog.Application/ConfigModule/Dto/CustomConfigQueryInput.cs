﻿using Z.Ddd.Common.ResultResponse.Pager;

namespace Z.SunBlog.Application.ConfigModule.Dto;

public class CustomConfigQueryInput : Pagination
{
    /// <summary>
    /// 配置名称
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 配置唯一编码
    /// </summary>
    public string? Code { get; set; }
}