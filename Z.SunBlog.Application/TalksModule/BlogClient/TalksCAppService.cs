﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Z.Ddd.Common.DomainServiceRegister;
using Z.Ddd.Common.Extensions;
using Z.Ddd.Common.ResultResponse;
using Z.Ddd.Common.UserSession;
using Z.EntityFrameworkCore;
using Z.EntityFrameworkCore.Extensions;
using Z.SunBlog.Application.AlbumsModule.BlogClient.Dto;
using Z.SunBlog.Application.TalksModule.BlogClient.Dto;
using Z.SunBlog.Core.AlbumsModule.DomainManager;
using Z.SunBlog.Core.CommentsModule;
using Z.SunBlog.Core.CommentsModule.DomainManager;
using Z.SunBlog.Core.PicturesModule.DomainManager;
using Z.SunBlog.Core.PraiseModule;
using Z.SunBlog.Core.PraiseModule.DomainManager;
using Z.SunBlog.Core.TalksModule.DomainManager;
using Z.Ddd.Common.Entities.Enum;

namespace Z.SunBlog.Application.TalksModule.BlogClient
{
    /// <summary>
    /// 说说前台管理
    /// </summary>
    public class TalksCAppService : ApplicationService, ITalksCAppService
    {
        private readonly ITalksManager _talksManager;
        private readonly IUserSession _userSession;
        private readonly IPraiseManager _praiseManager;
        private readonly ICommentsManager _commentsManager;
        public TalksCAppService(
            IServiceProvider serviceProvider,
            IUserSession userSession,
            ITalksManager talksManager,
            IPraiseManager praiseManager,
            ICommentsManager commentsManager) : base(serviceProvider)
        {
            _userSession = userSession;
            _talksManager = talksManager;
            _praiseManager = praiseManager;
            _commentsManager = commentsManager;
        }

        /// <summary>
        /// 说说列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<TalksOutput>> GetList([FromBody] Pagination dto)
        {

            var praiseList = _praiseManager.QueryAsNoTracking;

            string userId = _userSession.UserId;
            return await _talksManager.QueryAsNoTracking.Where(x => x.Status == AvailabilityStatus.Enable)
                  .OrderByDescending(x => x.IsTop)
                  .Select(x => new TalksOutput
                  {
                      Id = x.Id,
                      IsTop = x.IsTop,
                      Content = x.Content.Replace("\"", ""),
                      Images = x.Images,
                      Upvote = praiseList.Where(p => p.ObjectId == x.Id).Count(),
                      Comments = _commentsManager.QueryAsNoTracking.Where(c => c.ModuleId == x.Id && c.RootId == null).Count(),
                      IsPraise = praiseList.Where(p => p.ObjectId == x.Id && p.AccountId == userId).Any(),
                      CreatedTime = x.CreationTime
                  }).ToPagedListAsync(dto);
        }

        /// <summary>
        /// 说说详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<TalkDetailOutput> TalkDetail([FromQuery] Guid id)
        {
            string userId = _userSession.UserId;
            var praiseList = _praiseManager.QueryAsNoTracking;
            return await _talksManager.QueryAsNoTracking
                .Where(x => x.Id == id)
                .Select(x => new TalkDetailOutput
                {
                    Id = x.Id,
                    Content = x.Content,
                    Images = x.Images,
                    IsTop = x.IsTop,
                    IsAllowComments = x.IsAllowComments,
                    IsPraise = praiseList.Where(p => p.ObjectId == x.Id && p.AccountId == userId).Any(),
                    Upvote = praiseList.Where(p => p.ObjectId == x.Id).Count(),
                    CreatedTime = x.CreationTime
                }).FirstAsync();
        }
    }


}
