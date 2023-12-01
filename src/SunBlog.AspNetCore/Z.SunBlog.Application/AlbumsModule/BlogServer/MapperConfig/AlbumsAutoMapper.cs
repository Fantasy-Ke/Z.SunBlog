// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Z.SunBlog.Application.AlbumsModule.BlogServer.Dto;
using Z.SunBlog.Core.AlbumsModule;

namespace Z.SunBlog.Application.AlbumsModule.BlogServer.MapperConfig
{
    public static class AlbumsAutoMapper
    {
        /// <summary>
        /// 具体映射规则
        /// </summary>
        /// <param name="configuration"></param>
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Albums, CreateOrUpdateAlbumsInput>().ReverseMap();
        }
    }
}
