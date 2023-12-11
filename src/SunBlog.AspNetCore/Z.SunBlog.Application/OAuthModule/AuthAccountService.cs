
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.EntityFrameworkCore.Extensions;
using Z.Module.DependencyInjection;
using Z.SunBlog.Application.OAuthModule.Dto;
using Z.SunBlog.Core.AuthAccountModule.DomainManager;
using Z.SunBlog.Core.SharedDto;
using Z.SunBlog.Core.UserModule.DomainManager;
using Z.SunBlog.Core.AuthAccountModule;
using Z.Fantasy.Core.ResultResponse.Pager;

namespace Z.SunBlog.Application.OAuthModule;


public interface IAuthAccountAppService : IApplicationService, ITransientDependency
{
    Task<PageResult<AuthAccountPageOutput>> GetList([FromBody] AuthAccountPageQueryInput dto);

    Task SetBlogger(string id);

    Task Delete(string id);
}

/// <summary>
/// 博客授权用户
/// </summary>
public class AuthAccountAppService :ApplicationService, IAuthAccountAppService
{

    private readonly IAuthAccountDomainManager _authAccountDomainManager;
    public AuthAccountAppService(IServiceProvider serviceProvider, IAuthAccountDomainManager authAccountDomainManager) : base(serviceProvider)
    {
        _authAccountDomainManager = authAccountDomainManager;
    }

    /// <summary>
    /// 博客授权用户列表
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [DisplayName("博客授权用户列表")]
    [HttpPost]
    public async Task<PageResult<AuthAccountPageOutput>> GetList([FromBody] AuthAccountPageQueryInput dto)
    {
        return await _authAccountDomainManager.QueryAsNoTracking
            .WhereIf(!string.IsNullOrWhiteSpace(dto.Name), x => x.Name.Contains(dto.Name))
            .OrderByDescending(x => x.Id)
             .Select(x => new AuthAccountPageOutput
             {
                 Id = x.Id,
                 Name = x.Name,
                 Gender = x.Gender,
                 Type = x.Type,
                 IsBlogger = x.IsBlogger,
                 Avatar = x.Avatar,
                 CreatedTime = x.CreationTime
             }).ToPagedListAsync(dto);
    }

    /// <summary>
    /// 设置博主
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [DisplayName("设置博主")]
    [HttpPatch]
    public async Task SetBlogger(string id)
    {
        var entity = await _authAccountDomainManager.FindByIdAsync(id);

        entity.IsBlogger = !entity.IsBlogger;

        await _authAccountDomainManager.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除博客用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [DisplayName("删除博客用户")]
    [HttpDelete]
    public async Task Delete(string id)
    {
        await _authAccountDomainManager.DeleteAsync(id);
    }
}