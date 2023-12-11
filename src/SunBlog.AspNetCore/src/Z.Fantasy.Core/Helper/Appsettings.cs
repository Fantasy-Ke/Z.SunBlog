using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Z.Fantasy.Core.Exceptions;

namespace Z.Fantasy.Core.Helper
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public class AppSettings
    {
        public static IConfiguration Configuration;

        /// <summary>
        /// 根服务
        /// </summary>
        internal static IServiceProvider RootServices;

        /// <summary>
        /// 获取泛型主机环境
        /// </summary>
        internal static IHostEnvironment HostEnvironment;



        static string contentPath { get; set; }


        public AppSettings(string contentPath)
        {
            string Path = "appsettings.json";

            //如果你把配置文件 是 根据环境变量来分开了，可以这样写
            //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";

            Configuration = new ConfigurationBuilder()
                .SetBasePath(contentPath)
                .Add(new JsonConfigurationSource
                {
                    Path = Path,
                    Optional = false,
                    ReloadOnChange = true
                }) //这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
                .Build();
        }

        public AppSettings(WebApplicationBuilder builder)
        {
            Configuration = builder.Configuration;
            RootServices = builder.Services.BuildServiceProvider();
        }

        public static void ConfigurationProvider(IServiceCollection serviceCollection, string path = "appsettings.json")
        {

            //如果你把配置文件 是 根据环境变量来分开了，可以这样写
            //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";

            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .Add(new JsonConfigurationSource
                {
                    Path = path,
                    Optional = false,
                    ReloadOnChange = true
                }) //这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
                .Build();

            RootServices = serviceCollection.BuildServiceProvider();

        }
        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections">节点配置</param>
        /// <returns></returns>
        public static string app(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static List<T> app<T>(params string[] sections)
        {
            List<T> list = new List<T>();
            // 引用 Microsoft.Extensions.Configuration.Binder 包
            Configuration.Bind(string.Join(":", sections), list);
            return list;
        }


        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static T AppOption<T>(string sectionsPath)
        {
            T result;
            try
            {
                result = Configuration.GetSection(sectionsPath).Get<T>()!;
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }




        /// <summary>
        /// 根据路径  configuration["App:Name"];
        /// </summary>
        /// <param name="sectionsPath"></param>
        /// <returns></returns>
        public static string GetValue(string sectionsPath)
        {
            try
            {
                return Configuration[sectionsPath];
            }
            catch (Exception)
            {
            }

            return "";
        }

        /// <summary>
        /// 获取静态文件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IHostEnvironment Environment()
        {
            return RootServices.GetRequiredService<IHostEnvironment>();
        }
    }
}