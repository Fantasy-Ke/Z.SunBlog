// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Z.Fantasy.Core.Entities.EntityLog;
using Z.SunBlog.Application.LogsModule.ExceptionlogServer.Dto;
using Z.SunBlog.Core.MenuModule;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.LogsModule.ExceptionlogServer.MapperConfig
{
    public static class ExceptionlogAutoMapper
    {
        /// <summary>
        /// 具体映射规则
        /// </summary>
        /// <param name="configuration"></param>
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ZExceptionLog, ExceptionlogOutput>().ReverseMap();
        }
    }
}
