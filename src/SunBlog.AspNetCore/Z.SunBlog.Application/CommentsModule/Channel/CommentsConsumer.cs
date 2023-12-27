using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Z.Module.DependencyInjection;
using Z.RabbitMQ;
using Z.SunBlog.Core.CommentsModule;
using Z.SunBlog.Core.CommentsModule.DomainManager;

namespace Z.SunBlog.Application.CommentsModule.Channel
{
    /// <summary>
    /// 
    /// </summary>
    public class CommentsConsumer : RabbitConsumerAsync<Comments>, ITransientDependency
    {
        private readonly ICommentsManager _commentsManager;
        private readonly ILogger<CommentsConsumer> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="loggerComments"></param>
        /// <param name="commentsManager"></param>
        public CommentsConsumer(
            IServiceProvider serviceProvider,
            ILogger<RabbitConsumer<Comments>> loggerComments
,
            ICommentsManager commentsManager,
            ILogger<CommentsConsumer> logger)
            : base(serviceProvider, loggerComments)
        {
            _commentsManager = commentsManager;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgs"></param>
        public override Task Exec(Comments eventArgs)
        {
            throw new NotImplementedException();
            _logger.LogWarning($"传递实体数据{JsonConvert.SerializeObject(eventArgs)}");
            
            return Task.CompletedTask;
            //_commentsManager.CreateAsync(eventArgs).GetAwaiter();
        }
    }
}
