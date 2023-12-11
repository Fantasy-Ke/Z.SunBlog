### 🎨模块化类库，参照AbpVnext实现，现已正常使用

- Z.Module`1.0.3`
    - 添加`autofac`禁止注入特性
        ``` C#
        namespace Z.Module.Modules;

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
        public class DisablePropertyInjectionAttribute : Attribute
        {

        }

        ```
    - 添加Module获取所有Assemblies，Helper类`ZModuleHelper`
        ``` C#
        public static Assembly[] GetAllAssemblies(Type moduleType)
        {
            var assemblies = new List<Assembly>();

            var additionalAssemblyDescriptors = moduleType
                .GetCustomAttributes()
                .OfType<IAdditionalModuleAssemblyProvider>();

            foreach (var descriptor in additionalAssemblyDescriptors)
            {
                foreach (var assembly in descriptor.GetAssemblies())
                {
                    assemblies.AddIfNotContains(assembly);
                }
            }

            assemblies.Add(moduleType.Assembly);

            return assemblies.ToArray();
        }

        ```

- Z.Module`1.0.2`
    - 添加异步管道加载
    ``` C#
    var app = builder.Build();

    await app.InitApplicationAsync();

    app.Run();

    ```
    - 重写`OnInitApplicationAsync` `PostInitApplicationAsync`
    ``` C#
    public virtual Task OnInitApplicationAsync(InitApplicationContext context)
    {
        return Task.CompletedTask;
    }

    public virtual Task PostInitApplicationAsync(InitApplicationContext context)
    {
        return Task.CompletedTask;
    }
    ```

    - 之前的同步管道加载
    ``` C#
    var app = builder.Build();

    await app.InitApplication();

    app.Run();

    ```

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
