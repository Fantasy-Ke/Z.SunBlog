// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Z.SunBlog.Application.PictureModule.BlogServer.Dto;
using Z.SunBlog.Core.PicturesModule;

namespace Z.SunBlog.Application.PictureModule.BlogServer.MapperConfig
{
    public static class PictureAutoMapper
    {
        /// <summary>
        /// 具体映射规则
        /// </summary>
        /// <param name="configuration"></param>
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Pictures, AddPictureInput>().ReverseMap();
        }
    }
}
