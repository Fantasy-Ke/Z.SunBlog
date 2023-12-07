using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Module.Modules;
using Z.Module.Modules.interfaces;

namespace Z.Module.Extensions
{
    public static class ZApplicationBuilderExtensions
    {


        public static void CheckNull(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException("IAppBuilder为空");
        }

        public static IApplicationBuilder GetApplicationBuilder(this InitApplicationContext context)
        {
            return context.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;
        }


        public static void InitApplication(this IApplicationBuilder app)
        {

            InitBaseSetting(app);
            var runner = app.ApplicationServices.GetRequiredService<IZApplicationServiceProvider>();
            runner.Initialize(app.ApplicationServices);
        }

        public static async Task InitApplicationAsync(this IApplicationBuilder app)
        {
            InitBaseSetting(app);
            var runner = app.ApplicationServices.GetRequiredService<IZApplicationServiceProvider>();
            await runner.InitializeAsync(app.ApplicationServices);
        }


        /// <summary>
        /// 初始化基础
        /// </summary>
        /// <param name="app"></param>
        private static void InitBaseSetting(IApplicationBuilder app)
        {
            app.CheckNull();
            app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
            app.ApplicationServices.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value = app;

            
        }
    }
}
