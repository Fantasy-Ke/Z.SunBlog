using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Z.EntityFrameworkCore.Extensions;
using Z.Fantasy.Core.DomainServiceRegister;
using Z.Fantasy.Core.ResultResponse.Pager;
using Z.SunBlog.Application.LogsModule.ExceptionlogServer.Dto;
using Z.SunBlog.Core.LogsModule.ExceptionlogManager;
using Z.SunBlog.Core.SharedDto;

namespace Z.SunBlog.Application.LogsModule.ExceptionlogServer
{
    /// <summary>
    /// 异常日志
    /// </summary>
    public class ExceptionlogAppService : ApplicationService, IExceptionlogAppService
    {
        private readonly IExceptionLogManager _exceptionLogManager;

        /// <summary>
        /// 异常日志构造函数
        /// </summary>
        public ExceptionlogAppService(
            IServiceProvider serviceProvider,
            IExceptionLogManager exceptionLogManager
        )
            : base(serviceProvider)
        {
            _exceptionLogManager = exceptionLogManager;
        }

        [HttpPost]
        public async Task<PageResult<ExceptionlogOutput>> GetPage(
            [FromBody] ExceptionlogQueryInput input
        )
        {
            var extloglist = await _exceptionLogManager
                .QueryAsNoTracking.WhereIf(
                    !string.IsNullOrWhiteSpace(input.name),
                    x =>
                        x.RequestUri.Contains(input.name)
                        || x.StackTrace.Contains(input.name)
                        || x.Source.Contains(input.name)
                )
                .OrderByDescending(x => x.CreationTime)
                .Count(out var totalCount)
                .Page(input.PageNo, input.PageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)input.PageSize);
            return new PageResult<ExceptionlogOutput>()
            {
                PageNo = input.PageNo,
                PageSize = input.PageSize,
                Rows = ObjectMapper.Map<List<ExceptionlogOutput>>(extloglist),
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
            await _exceptionLogManager.DeleteAsync(dto.Id);
        }
    }
}
