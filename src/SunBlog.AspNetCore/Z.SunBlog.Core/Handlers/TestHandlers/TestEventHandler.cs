﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Minio;
using Z.EventBus.Handlers;
using Z.Module.DependencyInjection;

namespace Z.SunBlog.Core.Handlers.TestHandlers
{
    public class TestEventHandler : IEventHandler<TestDto>, ITransientDependency
    {
        private Microsoft.Extensions.Logging.ILogger _logger;
        public TestEventHandler(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<TestEventHandler>();
        }
        public Task HandelrAsync(TestDto eto)
        {
            _logger.LogInformation($"{typeof(TestDto).Name}--{eto.Name}--{eto.Description}");
            return Task.CompletedTask;
        }
    }
}
