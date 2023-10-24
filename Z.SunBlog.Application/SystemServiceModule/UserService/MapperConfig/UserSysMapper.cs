// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Z.Ddd.Common.Entities.Users;
using Z.SunBlog.Application.OAuthModule.Dto;
using Z.SunBlog.Application.SystemServiceModule.UserService.Dto;
using Z.SunBlog.Application.TagsModule.BlogServer.Dto;
using Z.SunBlog.Core.ArticleModule;
using Z.SunBlog.Core.FriendLinkModule;
using Z.SunBlog.Core.TagModule;

namespace Z.SunBlog.Application.SystemServiceModule.UserService.MapperConfig
{
    public static class UserSysMapper
    {
        /// <summary>
        /// 具体映射规则
        /// </summary>
        /// <param name="configuration"></param>
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<AddSysUserInput, ZUserInfo>().ReverseMap();
            configuration.CreateMap<UpdateSysUserInput, ZUserInfo>().ReverseMap();
        }
    }
}