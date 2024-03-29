﻿using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Z.SunBlog.Application.ConfigModule.Dto;

public class CustomConfigSetJsonInput
{
    /// <summary>
    /// 自定义配置ID
    /// </summary>
    [Required(ErrorMessage = "缺少必要参数")]
    public Guid Id { get; set; }

    /// <summary>
    /// 表单设计
    /// </summary>
    [Required(ErrorMessage = "请设计表单")]
    public string Json { get; set; }
}