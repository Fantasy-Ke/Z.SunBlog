// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Z.Fantasy.Core.Entities.Roles;
using Z.SunBlog.Application.SystemServiceModule.RoleService.Dto;

namespace Z.SunBlog.Application.SystemServiceModule.RoleService.MapperConfig
{
    public static class RoleSysMapper
    {
        /// <summary>
        /// 具体映射规则
        /// </summary>
        /// <param name="configuration"></param>
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ZRoleInfo, AddRoleInput>().ReverseMap();
            configuration.CreateMap<ZRoleInfo, UpdateSysRoleInput>().ReverseMap();
        }
    }
}
