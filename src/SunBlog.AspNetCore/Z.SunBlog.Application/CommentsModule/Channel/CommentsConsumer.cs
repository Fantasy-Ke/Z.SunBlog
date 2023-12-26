using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Z.Module.DependencyInjection;
using Z.RabbitMQ;
using Z.SunBlog.Core.CommentsModule;
using Z.SunBlog.Core.CommentsModule.DomainManager;

namespace Z.SunBlog.Application.CommentsModule.Channel
{
    /// <summary>
    /// 
    /// </summary>
    public class CommentsConsumer : RabbitConsumer<Comments>, ITransientDependency
    {
        private readonly ICommentsManager _commentsManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        /// <param name="commentsManager"></param>
        public CommentsConsumer(
            IServiceProvider serviceProvider,
            ILogger<RabbitConsumer<Comments>> logger
,
            ICommentsManager commentsManager)
            : base(serviceProvider, logger)
        {
            _commentsManager = commentsManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventArgs"></param>
        public override void Exec(Comments eventArgs)
        {
            _commentsManager.CreateAsync(eventArgs).GetAwaiter();
        }
    }
}
