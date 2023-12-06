using Z.EventBus.Extensions;
using Z.Module;
using Z.Module.Modules;
using Z.SunBlog.Common;
using Z.SunBlog.Core.Handlers.FileHandlers;

namespace Z.SunBlog.Core
{
    [DependOn(typeof(SunBlogCommonModule))]
    public class SunBlogCoreModule : ZModule
    {
        public override void ConfigureServices(ServiceConfigerContext context)
        {
            // 注入事件总线
            context.Services.AddEventBus();


            context.Services.EventBusSubscribes(c =>
            {
                c.Subscribe<FileEventDto, FileEventHandler>();
            });
        }

        public override void OnInitApplication(InitApplicationContext context)
        {
            context.ServiceProvider.InitChannles();
        }
    }
}