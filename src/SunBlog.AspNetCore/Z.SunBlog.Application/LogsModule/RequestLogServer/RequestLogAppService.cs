using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Z.EntityFrameworkCore.Extensions;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.LogsModule.RequestLogServer.Dto;
using Z.SunBlog.Core.LogsModule.RequestLogManager;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.LogsModule.RequestLogServer
{
    /// <summary>
    /// 异常日志
    /// </summary>
    public class RequestLogAppService : ApplicationService, IRequestLogAppService
    {
        private readonly IRequestLogManager _requestLogManager;
        /// <summary>
        /// 异常日志构造函数
        /// </summary>
        public RequestLogAppService(IServiceProvider serviceProvider,
            IRequestLogManager requestLogManager) : base(serviceProvider)
        {
            _requestLogManager = requestLogManager;
        }

        [HttpPost]
        public async Task<PageResult<RequestLogOutput>> GetPage([FromBody] RequestLogQueryInput input)
        {
            var reqloglist = await _requestLogManager.QueryAsNoTracking
                .WhereIf(!string.IsNullOrWhiteSpace(input.name), 
                x => x.RequestUri.Contains(input.name) || 
                x.RequestType.Contains(input.name) || 
                x.ResponseData.Contains(input.name))
                .OrderByDescending(x => x.CreationTime)
                .Count(out var totalCount)
                .Page(input.PageNo,input.PageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)input.PageSize);
            var items = ObjectMapper.Map<List<RequestLogOutput>>(reqloglist);
            return new PageResult<RequestLogOutput>()
            {
                PageNo = input.PageNo,
                PageSize = input.PageSize,
                Rows = items,
                Total = (int)totalCount,
                Pages = totalPages,
            };
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteAsync(KeyDto dto)
        {
            await _requestLogManager.DeleteAsync(dto.Id);
        }
    }
}
