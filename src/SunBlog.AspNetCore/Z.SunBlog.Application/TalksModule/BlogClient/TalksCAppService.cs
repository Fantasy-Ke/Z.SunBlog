﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.EntityFrameworkCore.Extensions;
using Z.SunBlog.Application.TalksModule.BlogClient.Dto;
using Z.SunBlog.Core.CommentsModule.DomainManager;
using Z.SunBlog.Core.PraiseModule.DomainManager;
using Z.SunBlog.Core.TalksModule.DomainManager;
using Z.Fantasy.Core.Entities.Enum;
using Z.Fantasy.Core.ResultResponse.Pager;

namespace Z.SunBlog.Application.TalksModule.BlogClient
{
    /// <summary>
    /// 说说前台管理
    /// </summary>
    public class TalksCAppService : ApplicationService, ITalksCAppService
    {
        private readonly ITalksManager _talksManager;
        private readonly IPraiseManager _praiseManager;
        private readonly ICommentsManager _commentsManager;
        public TalksCAppService(
            IServiceProvider serviceProvider,
            ITalksManager talksManager,
            IPraiseManager praiseManager,
            ICommentsManager commentsManager) : base(serviceProvider)
        {
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

            string userId = UserService.UserId;
            return await _talksManager.QueryAsNoTracking.Where(x => x.Status == AvailabilityStatus.Enable)
                  .OrderByDescending(x => x.IsTop)
                  .Select(x => new TalksOutput
                  {
                      Id = x.Id,
                      IsTop = x.IsTop,
                      Content = x.Content.Replace("\"", ""),
                      Images = x.Images,
                      Upvote = praiseList.Count(p => p.ObjectId == x.Id),
                      Comments = _commentsManager.QueryAsNoTracking.Count(c => c.ModuleId == x.Id && c.RootId == null),
                      IsPraise = praiseList.Any(p => p.ObjectId == x.Id && p.AccountId == userId),
                      CreatedTime = x.CreationTime
                  }).ToPagedListAsync(dto);
        }

        /// <summary>
        /// 说说详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TalkDetailOutput> TalkDetail(Guid id)
        {
            string userId = UserService.UserId;
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
                    IsPraise = praiseList.Any(p => p.ObjectId == x.Id && p.AccountId == userId),
                    Upvote = praiseList.Count(p => p.ObjectId == x.Id),
                    CreatedTime = x.CreationTime
                }).FirstAsync();
        }
    }


}
