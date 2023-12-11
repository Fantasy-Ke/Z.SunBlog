using Microsoft.AspNetCore.Mvc;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.SunBlog.Application.CategoryModule.BlogServer.Dto;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.CategoryModule.BlogServer
{
    /// <summary>
    /// 文章栏目管理
    /// </summary>
    public interface ICategorySAppService : IApplicationService
    {
        Task<List<CategoryPageOutput>> GetPage([FromQuery] string name);

        Task<List<TreeSelectOutput>> TreeSelect();

        Task UpdateCategory(UpdateCategoryInput dto);

        Task AddCategory(AddCategoryInput dto);

        Task Delete(KeyDto dto);

    }
}
