﻿using Microsoft.AspNetCore.Mvc;
using Z.Ddd.Common.DomainServiceRegister;
using Z.Ddd.Common.ResultResponse;
using Z.Ddd.Common.UserSession;
using Z.EntityFrameworkCore.Extensions;
using Z.SunBlog.Application.TalksModule.BlogServer.Dto;
using Z.SunBlog.Core.PraiseModule.DomainManager;
using Z.SunBlog.Core.SharedDto;
using Z.SunBlog.Core.TalksModule;
using Z.SunBlog.Core.TalksModule.DomainManager;

namespace Z.SunBlog.Application.TalksModule.BlogServer
{
    /// <summary>
    /// 说说后台管理
    /// </summary>
    public class TalksSAppService : ApplicationService, ITalksSAppService
    {
        private readonly ITalksManager _talksManager;
        private readonly IUserSession _userSession;
        private readonly IPraiseManager _praiseManager;
        public TalksSAppService(
            IServiceProvider serviceProvider, ITalksManager talksManager, IUserSession userSession, IPraiseManager praiseManager) : base(serviceProvider)
        {
            _talksManager = talksManager;
            _userSession = userSession;
            _praiseManager = praiseManager;
        }

        /// <summary>
        /// 添加修改说说
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task CreateOrUpdate(CreateOrUpdateTalksInput dto)
        {
            if (dto.Id != null && dto.Id != Guid.Empty)
            {
                await Update(dto);
                return;
            }

            await Create(dto);
        }

        /// <summary>
        /// 说说分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<TalksPageOutput>> GetPage([FromBody] TalksPageQueryInput dto)
        {
            string userId = _userSession.UserId;
            return await _talksManager.QueryAsNoTracking
                  .WhereIf(!string.IsNullOrWhiteSpace(dto.Keyword), x => x.Content.Contains(dto.Keyword))
                  .OrderByDescending(x => x.Id)
                  .Select(x => new TalksPageOutput
                  {
                      Id = x.Id,
                      Status = x.Status,
                      Content = x.Content,
                      Images = x.Images,
                      IsAllowComments = x.IsAllowComments,
                      IsTop = x.IsTop,
                      IsPraise = _praiseManager.QueryAsNoTracking.Where(p => p.ObjectId == x.Id && p.AccountId == userId).Any(),
                      CreatedTime = x.CreationTime
                  }).ToPagedListAsync(dto);
        }





        /// <summary>
        /// 添加说说
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private async Task Create(CreateOrUpdateTalksInput dto)
        {
            var talks = ObjectMapper.Map<Talks>(dto);
            await _talksManager.Create(talks);
        }

        /// <summary>
        /// 更新说说
        /// </summary>
        /// <returns></returns>
        private async Task Update(CreateOrUpdateTalksInput dto)
        {
            var talks = await _talksManager.FindByIdAsync(dto.Id!.Value);

            ObjectMapper.Map(dto, talks);

            await _talksManager.Update(talks!);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task DeleteAsync(KeyDto dto)
        {
            await _talksManager.Delete(dto.Id);
        }
    }


}
