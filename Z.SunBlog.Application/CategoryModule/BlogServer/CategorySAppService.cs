﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using Z.Ddd.Common.DomainServiceRegister;
using Z.Ddd.Common.Exceptions;
using Z.Ddd.Common.UserSession;
using Z.SunBlog.Application.CategoryModule.BlogServer.Dto;
using Z.SunBlog.Core.CategoriesModule;
using Z.SunBlog.Core.CategoriesModule.DomainManager;
using Z.SunBlog.Core.MenuModule;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.CategoryModule.BlogServer
{
    /// <summary>
    /// 相册图片管理
    /// </summary>
    public class CategorySAppService : ApplicationService, ICategorySAppService
    {
        private readonly ICategoriesManager _categoriesManager;
        private readonly IUserSession _userSession;
        public CategorySAppService(
            IServiceProvider serviceProvider, IUserSession userSession, ICategoriesManager categoriesManager) : base(serviceProvider)
        {
            _userSession = userSession;
            _categoriesManager = categoriesManager;
        }

        /// <summary>
        /// 添加文章栏目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DisplayName("添加文章栏目")]
        [HttpPost]
        public async Task AddCategory(AddCategoryInput dto)
        {
            var entity = ObjectMapper.Map<Categories>(dto);
            await _categoriesManager.Create(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Delete(KeyDto dto)
        {
            await _categoriesManager.Delete(x => x.Id == dto.Id);
        }

        /// <summary>
        /// 文章栏目列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [DisplayName("文章栏目列表")]
        [HttpGet]
        public async Task<List<CategoryPageOutput>> GetPage([FromQuery] string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var list = await _categoriesManager.QueryAsNoTracking.Where(x => x.Name.Contains(name)).ToListAsync();
                return ObjectMapper.Map<List<CategoryPageOutput>>(list);
            }

            var categoriesList = await _categoriesManager.QueryAsNoTracking
                .Where(x => x.ParentId == null).OrderBy(x => x.Sort)
                .ToListAsync();

            await BuildCategories(categoriesList);

            return ObjectMapper.Map<List<CategoryPageOutput>>(categoriesList);
        }

        /// <summary>
        /// 获取文章栏目下拉选项
        /// </summary>
        /// <returns></returns>
        [DisplayName("获取文章栏目下拉选项")]
        [HttpGet]
        public async Task<List<TreeSelectOutput>> TreeSelect()
        {
            var list = await _categoriesManager.QueryAsNoTracking
            .OrderBy(x => x.Sort).Where(p => p.ParentId == null).ToListAsync();
            await BuildCategories(list);
            return ObjectMapper.Map<List<TreeSelectOutput>>(list);
        }

        /// <summary>
        /// 更新文章栏目
        /// </summary>
        /// <returns></returns>
        [DisplayName("更新文章栏目")]
        [HttpPut]
        public async Task UpdateCategory(UpdateCategoryInput dto)
        {
            var entity = await _categoriesManager.FindByIdAsync(dto.Id);
            if (entity == null) throw new UserFriendlyException("无效参数");
            ObjectMapper.Map(dto, entity);
            await _categoriesManager.Update(entity);
        }

        private async Task BuildCategories(List<Categories> categoriesPanentLists)
        {
            foreach (var categories in categoriesPanentLists)
            {
                var categoriesList = await _categoriesManager.QueryAsNoTracking
                    .Where(p => p.ParentId == categories.Id).ToListAsync();
                if (categoriesList.Any())
                {
                    categories.Children = categoriesList;
                    await BuildCategories(categoriesList);
                }
            }
        }
    }


}
