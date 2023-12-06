### 🎨模块化类库，参照AbpVnext实现，现已正常使用

- abp vnext 模块依赖（已完成）

  - ``````C#
    namespace Z.SunBlog.Host;
    
    [DependOn(typeof(SunBlogApplicationModule),
        typeof(SunBlogEntityFrameworkCoreModule))]
    public class SunBlogHostModule : ZModule
    {
        /// <summary>
        /// 服务配置
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ServiceConfigerContext context)
        {
            configuration = context.GetConfiguration();
            //....
        }
    
        /// <summary>
        /// 初始化应用
        /// </summary>
        /// <param name="context"></param>
        public override void OnInitApplication(InitApplicationContext context)
        {
            var app = context.GetApplicationBuilder();
        }
    }
    
  
- 自动注册（已完成）

  - ITransientDependency（瞬时）

    - ``````C#
      public class JwtTokenProvider : IJwtTokenProvider , ITransientDependency
      {
      
      public interface IJwtTokenProvider
      {
      }

  - ISingletonDependency（单例）

    - ``````C#
      public class JwtTokenProvider : IJwtTokenProvider , ISingletonDependency
      {
      
      public interface IJwtTokenProvider
      {
      }

  - IScopedDependency（作用域）

    - ``````c#
      public class JwtTokenProvider : IJwtTokenProvider , IScopedDependency
      {
      
      public interface IJwtTokenProvider
      {
      }
