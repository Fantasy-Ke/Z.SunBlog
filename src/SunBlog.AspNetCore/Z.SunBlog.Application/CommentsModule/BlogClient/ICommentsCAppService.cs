using Microsoft.AspNetCore.Mvc;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.CommentsModule.BlogClient.Dto;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.CommentsModule.BlogClient
{
    /// <summary>
    /// 文章管理
    /// </summary>
    public interface ICommentsCAppService : IApplicationService
    {
        Task<PageResult<CommentOutput>> GetList([FromBody] CommentPageQueryInput dto);


        Task<PageResult<ReplyOutput>> ReplyList([FromBody] CommentPageQueryInput dto);

        Task Add(AddCommentInput dto);
        Task<bool> Praise(KeyDto dto);


    }
}
