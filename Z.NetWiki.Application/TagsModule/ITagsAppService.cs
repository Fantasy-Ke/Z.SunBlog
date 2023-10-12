﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Ddd.Common.DomainServiceRegister;
using Z.Ddd.Common.Entities.Users;
using Z.Ddd.Common.ResultResponse;
using Z.NetWiki.Application.ArticleModule.Dto;
using Z.NetWiki.Application.TagsModule.Dto;
using Z.NetWiki.Application.UserModule.Dto;
using Z.NetWiki.Domain.SharedDto;

namespace Z.NetWiki.Application.TagsModule
{
    /// <summary>
    /// 文章管理
    /// </summary>
    public interface ITagsAppService : IApplicationService
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task DeleteAsync(KeyDto dto);

        /// <summary>
        /// 文章列表分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PageResult<TagsPageOutput>> GetPage([FromQuery] TagsPageQueryInput dto);

        /// <summary>
        /// 创建修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateTagInput dto);

        /// <summary>
        /// 文章标签下拉选项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        Task<List<SelectOutput>> Select();

    }
}
