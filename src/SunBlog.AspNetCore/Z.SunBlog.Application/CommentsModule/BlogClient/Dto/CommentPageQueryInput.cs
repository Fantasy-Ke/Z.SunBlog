using Z.Fantasy.Core.ResultResponse.Pager;

namespace Z.SunBlog.Application.CommentsModule.BlogClient.Dto;

public class CommentPageQueryInput : Pagination
{
    /// <summary>
    /// 对应模块ID或评论ID（null表留言，0代表友链的评论）
    /// </summary>
    public Guid? Id { get; set; }
}