﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Dynamic.Core;
using Z.Ddd.Common;
using Z.Ddd.Common.DomainServiceRegister;
using Z.Ddd.Common.ResultResponse;
using Z.Ddd.Common.UserSession;
using Z.EntityFrameworkCore.Extensions;
using Z.SunBlog.Application.CommentsModule.BlogClient.Dto;
using Z.SunBlog.Core.AuthAccountModule.DomainManager;
using Z.SunBlog.Core.CommentsModule;
using Z.SunBlog.Core.CommentsModule.DomainManager;
using Z.SunBlog.Core.PraiseModule;
using Z.SunBlog.Core.PraiseModule.DomainManager;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.CommentsModule.BlogClient
{
    /// <summary>
    /// 评论
    /// </summary>
    public class CommentsCAppService : ApplicationService, ICommentsCAppService
    {
        private readonly ICommentsManager _commentsManager;
        private readonly IAuthAccountDomainManager _authAccountDomainManager;
        private readonly IPraiseManager _praiseManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserSession _userSession;

        public CommentsCAppService(
            IServiceProvider serviceProvider, ICommentsManager commentsManager, IAuthAccountDomainManager authAccountDomainManager, IPraiseManager praiseManager, IUserSession userSession, IHttpContextAccessor httpContextAccessor) : base(serviceProvider)
        {
            _commentsManager = commentsManager;
            _authAccountDomainManager = authAccountDomainManager;
            _praiseManager = praiseManager;
            _userSession = userSession;
            _httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// 评论、回复
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Add(AddCommentInput dto)
        {
            string address = _httpContextAccessor.HttpContext.GetGeolocation()?.Address;
            var comments = ObjectMapper.Map<Comments>(dto);
            comments.AccountId = _userSession.UserId;
            comments.IP = _httpContextAccessor.HttpContext.GetRemoteIp();
            comments.Geolocation = address;
            await _commentsManager.Create(comments);
        }


        /// <summary>
        /// 评论列表
        /// </summary>
        /// <returns></returns>
        public async Task<PageResult<CommentOutput>> GetList([FromBody] CommentPageQueryInput dto)
        {
            var praiseList = _praiseManager.QueryAsNoTracking;
            string userId = _userSession.UserId;
            var result = await _commentsManager.QueryAsNoTracking
                .GroupJoin(_authAccountDomainManager.QueryAsNoTracking,
                c => c.AccountId, au => au.Id,
                (c, au) => new { comment = c, auth = au })
                .SelectMany(a => a.auth.DefaultIfEmpty(), (m, n) => new
                {
                    comment = m.comment,
                    auth = n
                })
                .Where(c => c.comment.ModuleId == dto.Id && c.comment.RootId == null) //排除回复的评论
                .OrderByDescending(c => c.comment.Id)
                .Select(c => new CommentOutput
                {
                    Id = c.comment.Id,
                    Content = c.comment.Content,
                    PraiseTotal = praiseList.Where(x => x.ObjectId == c.comment.Id).Count(),
                    IsPraise = praiseList.Where(x => x.ObjectId == c.comment.Id && x.AccountId == userId).Any(),
                    ReplyCount = _commentsManager.QueryAsNoTracking.Where(s => s.RootId == c.comment.Id).Count(),
                    IP = c.comment.IP,
                    Avatar = c.auth.Avatar,
                    AccountId = c.auth.Id,
                    NickName = c.auth.Name,
                    IsBlogger = c.auth.IsBlogger,
                    Geolocation = c.comment.Geolocation,
                    CreatedTime = c.comment.CreationTime
                })
                //.Mapper(it =>
                //{
                //    if (it.ReplyCount > 0)
                //    {
                //        it.ReplyList = ReplyList(new CommentPageQueryInput()
                //        {
                //            PageNo = 1,
                //            Id = it.Id
                //        }).GetAwaiter().GetResult();
                //    }
                //})
                .ToPagedListAsync(dto);
            return result;
        }

        /// <summary>
        /// 点赞/取消点赞
        /// </summary>
        /// <param name="dto">对象ID</param>
        /// <returns></returns>
        public async Task Praise(KeyDto dto)
        {
            if (_praiseManager.QueryAsNoTracking.Any(x => x.ObjectId == dto.Id))
            {
                await _praiseManager.Delete(x => x.ObjectId == dto.Id);//"糟糕，取消失败了..."
            };
            var praise = new Praise()
            {
                AccountId = _userSession.UserId,
                ObjectId = dto.Id,
            };
            await _praiseManager.Create(praise);//"糟糕，点赞失败了..."
        }

        /// <summary>
        /// 回复分页
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<ReplyOutput>> ReplyList([FromBody] CommentPageQueryInput dto)
        {
            string userId = _userSession.UserId;
            return await _commentsManager.QueryAsNoTracking
                    .GroupJoin(_authAccountDomainManager.QueryAsNoTracking,
                    c => c.AccountId,
                    au => au.Id,
                    (c, au) => new { comment = c, auth = au })
                    .SelectMany(a => a.auth.DefaultIfEmpty(), (m, n) => new
                    {
                        comment = m.comment,
                        auth = n
                    })
                    .GroupJoin(_authAccountDomainManager.QueryAsNoTracking,
                    c => c.comment.ReplyAccountId,
                    au1 => au1.Id,
                    (c, a1) => new { comAuth = c, auth1 = a1 })
                    .SelectMany(a => a.auth1.DefaultIfEmpty(), (m, n) => new
                    {
                        comAuth = m.comAuth,
                        auth1 = n
                    })
                  .Where(c => c.comAuth.comment.RootId == dto.Id)
                  .OrderBy(c => c.comAuth.comment.Id)
                  .Select(c => new ReplyOutput
                  {
                      Id = c.comAuth.comment.Id,
                      Content = c.comAuth.comment.Content,
                      ParentId = c.comAuth.comment.ParentId,
                      AccountId = c.comAuth.comment.AccountId,
                      ReplyAccountId = c.comAuth.comment.ReplyAccountId,
                      IsBlogger = c.comAuth.auth.IsBlogger,
                      NickName = c.comAuth.auth.Name,
                      RelyNickName = c.auth1.Name,
                      RootId = c.comAuth.comment.RootId,
                      Avatar = c.comAuth.auth.Avatar,
                      PraiseTotal = _praiseManager.QueryAsNoTracking.Where(x => x.ObjectId == c.comAuth.comment.Id).Count(),
                      IsPraise = _praiseManager.QueryAsNoTracking.Where(x => x.ObjectId == c.comAuth.comment.Id && x.AccountId == userId).Any(),
                      IP = c.comAuth.comment.IP,
                      Geolocation = c.comAuth.comment.Geolocation,
                      CreatedTime = c.comAuth.comment.CreationTime
                  }).ToPagedListAsync(dto);
        }
    }


}
