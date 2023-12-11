// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Z.Fantasy.Core.Entities.Organizations;
using Z.SunBlog.Application.SystemServiceModule.OrganizationService.Dto;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.SystemServiceModule.OrganizationService.MapperConfig
{
    public static class OrganizationMapper
    {
        /// <summary>
        /// 具体映射规则
        /// </summary>
        /// <param name="configuration"></param>
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ZOrganization, SysOrgPageOutput>().ReverseMap();
            configuration.CreateMap<AddOrgInput, ZOrganization>().ReverseMap();
            configuration.CreateMap<UpdateOrgInput, ZOrganization>().ReverseMap();
            configuration.CreateMap<ZOrganization, TreeSelectOutput>()
                .ForMember(dest => dest.Value, src => src.MapFrom(c => c.Id))
                .ForMember(dest => dest.Label, src => src.MapFrom(c => c.Name));
        }
    }
}
