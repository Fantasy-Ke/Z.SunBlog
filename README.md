# 🧨Z.SunBlog

#### 该项目已通过阿里云仓库、GithubAction持续集成与部署（[Fantasy-Ke -Z.SunBlog.Blog](http://sunblog.zblog.love/)）

## 🎃.NET 8 后端框架
### 👕SunBlog.AspNetCore/src 后端自己搭建的框架

- 🛺.Net8
- EF Core 7 适配SqlServer和Mysql🎏
  - 仓储
  - 简单工作单元
- 模块化处理😇
- 简单EventBus 基于Channels 
    - 中间件接口请求日志使用`队列事件发布订阅`和`工作单元`写入数据库
- Autofac依赖注入，AOP注册处理
    - 横向处理`Application`
- AutoMapper模块化注入 (想改成 `Mapster` 对象映射 )🍔
- ⛑️`Serilog`json配置化
    - Seq日志中心面板
    - 控制台日志
    - 文件日志插入
    - 数据库日志写入
- 👒简单Minio存储桶
- 🎪简单Redis缓存
    - 使用FreeRedis组件包了一层而已，直接使用
- 动态api
- HangFire 后台任务使用🎢
    - job类继承`BackgroundJob`和`AsyncBackgroundJob`实现接口`Execute`和`ExecuteAsync`(模仿abp vNext 实现的BackgroundJob，简化版👽)
    - 注入`IBackgroundJobManager`调用接口。实现传参队列和延迟队列 任务
- SignalR实时通信🚋
    - 前端的实时通知推送
    - Redis 无序列表缓存
- RabbitMQ封装事件发布订阅🪇
    - 负载均衡
    - 存储链接，队列配置使用内存存储
    - Polly(指数退避策略   重试)
- 授权管理(部分)
- ShardingCore 分库分表、读写分离(待实现)🫥
- Ocelot  Gateway、Polly(熔断降级) (待实现)⛑️
- MongoDb(待实现) \(￣︶￣*\))

| Build | NuGet |
|---|--|
|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![](https://github.com/Fantasy-Ke/Z.SunBlog/workflows/build_deploy_action/badge.svg)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[![](https://img.shields.io/badge/Z.Module-1.0.4-blue.svg)](https://www.nuget.org/packages/Z.Module)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|
|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![](https://github.com/Fantasy-Ke/Z.SunBlog/workflows/build_deploy_action/badge.svg)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[![](https://img.shields.io/badge/Z.EventBus-1.0.1-orange.svg)](https://www.nuget.org/packages/Z.EventBus)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|
|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![](https://github.com/Fantasy-Ke/Z.SunBlog/workflows/build_deploy_action/badge.svg)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[![](https://img.shields.io/badge/Z.RabbitMQ-1.0.0-red.svg)](https://www.nuget.org/packages/Z.RabbitMQ)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|
|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![](https://github.com/Fantasy-Ke/Z.SunBlog/workflows/build_deploy_action/badge.svg)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[![](https://img.shields.io/badge/Z.OSSCore-1.0.0-green.svg)](https://www.nuget.org/packages/Z.OSSCore)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|

![image](https://github.com/Fantasy-Ke/Z.SunBlog/assets/85232349/02a8f744-221e-4985-a2e6-5e6862555677)
![image](https://github.com/Fantasy-Ke/Z.SunBlog/assets/85232349/eaab01c9-76f5-469d-b43c-c54b8ef09003)



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

### 🎡EF基础仓储实现 

- 实现了对`sqlserver`以及`Mysql`的支持

  - **使用SqlServer**

  - 依赖模块`ZSqlServerEntityFrameworkCoreModule`
  
  - ```C#
    namespace Z.SunBlog.EntityFrameworkCore
    {
        [DependOn(typeof(ZSqlServerEntityFrameworkCoreModule))]
        public class SunBlogEntityFrameworkCoreModule : ZModule
        {
            public override void ConfigureServices(ServiceConfigerContext context)
            {
              context.AddSqlServerEfCoreEntityFrameworkCore<SunBlogDbContext>(connectionString);
            }
        }
    }
    ```
    
  - **使用Mysql**
  
  - 依赖模块`ZMysqlEntityFrameworkCoreModule`

  - ``````c#
    namespace Z.SunBlog.EntityFrameworkCore
    {
        [DependOn(typeof(ZMysqlEntityFrameworkCoreModule))]
        public class SunBlogEntityFrameworkCoreModule : ZModule
        {
            public override void ConfigureServices(ServiceConfigerContext context)
            {
                context.AddMysqlEfCoreEntityFrameworkCore<SunBlogDbContext>(new Version(8, 0,21), connectionString);
            }
        }
    }
    ``````

### 🧦Serilog配置
- 引入`Z.Fantasy.Core.Serilog`命名空间

- 在`Program`中使用

    - ``````c#
        builder.Host.AddSerilogSetup(); //注册Serilog
        ``````

- 在管道中使用`UseSerilogRequestLogging`处理拦截请求信息

    1. `SerilogRequestUtility.HttpMessageTemplate` 模板信息

    2. `SerilogRequestUtility.GetRequestLevel` 请求拦截等级

    3. `SerilogRequestUtility.EnrichFromRequest` 信息获取方法

- ``````c#
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = SerilogRequestUtility.HttpMessageTemplate;
        options.GetLevel = SerilogRequestUtility.GetRequestLevel;
        options.EnrichDiagnosticContext = SerilogRequestUtility.EnrichFromRequest;
    });
    ``````

## 🥽前台Blog简介

![image](https://github.com/Fantasy-Ke/Z.SunBlog/assets/85232349/87ba7ffd-2c08-4e6e-a9bb-80813c05f947)

#### 分为两个框架

##### SunBlog.BlogVue.Nuxt

- SunBlog.BlogVue.Nuxt

  - **项目用 Nuxt3 + TypeScript  + Vuetify + Pinia！**

  - ### 🤩环境

    -  node 18+  
    -  pwsh core  
    -  yarn

    ### 🤯前端主要技术 

    所有技术都是当前最新的。

    - vue： ^3.3.4
    - typescript : ^5.2.2
    - nuxt: ^3.8.2
    - highlight.js: ^10.7.2
    - marked：^9.1.0

##### SunBlog.BlogVue

- SunBlog.BlogVue

  - **项目用 Vue3 + TypeScript + Vite4  + Vuetify + Pinia！**


    ### 🤩环境
    
    -  node 16+  
    -  pwsh core  
    -  yarn
    
    ### 🤯前端主要技术 
    
    所有技术都是当前最新的。
    
    - vue： ^3.3.4
    - typescript : ^5.2.2
    - vue-router : ^4.2.5
    - vite: ^4.4.11
    - vuex: ^4.0.0
    - axios: ^1.5.1
    - highlight.js: ^10.7.2
    - marked：^9.1.0



### 🐱‍👓前端代理类生成

- cd到目录src/SunBlog.BlogVue的nswag目录
- 调整codeGenerators后端api地址
  ![image](https://github.com/Fantasy-Ke/Z.SunBlog/assets/85232349/089a9897-81e0-4299-a31f-b8b4d80c50d4)

- 双击refresh.bat 执行 


## 🥎后台Admin简介

**项目用 Vue3 + TypeScript + Vite4 + Vuex4 + Vue-Router4 + element-plus ！**

![image](https://github.com/Fantasy-Ke/Z.SunBlog/assets/85232349/01b8f982-6fa0-4cf9-9d8d-33e2a35a295f)


### 🙄环境

-  node 16+  
-  pwsh core  
-  yarn

### 🤡前端主要技术 

所有技术都是当前最新的。

- vue： ^3.3.4
- typescript : ^5.2.2
- element-plus: ^2.3.14
- vue-router : ^4.2.5
- vite: ^4.4.11
- vuex: ^4.0.0
- axios: ^1.5.1
- highlight.js: ^10.7.2
- marked：^9.1.0


### 🐱‍💻前端代理类生成

- cd到目录src/SunBlog.AdminVue 的 nswag目录
- 调整codeGenerators后端api地址
 ![image](https://github.com/Fantasy-Ke/Z.SunBlog/assets/85232349/3cc72624-6d5c-4918-872e-e3f68e0a7347)

- 双击refresh.bat 执行 


## 🍟感谢
- [Nuxt](https://nuxt.com.cn/)
  - Nuxt是一个 开源框架 ，使得Web开发变得直观且强大。可以自信地创建高性能和生产级别的全栈Web应用和网站。
- [Easy.Admin](https://gitee.com/miss_you/easy-admin)
  - SunBlog网站前后台基于该开源项目编写，在线网址：https://www.okay123.top/
- [Vue](https://cn.vuejs.org/)
  - 网站前后台前端使用Vue搭建
- [element-plus](https://element-plus.gitee.io/zh-CN/) 
  - Vue组件库
- [vue-next-admin](https://gitee.com/lyt-top/vue-next-admin)
  - 后台前端框架
- [vue-toastification](https://github.com/Maronato/vue-toastification)
  - 一个弹窗组件
- [OnceMi.AspNetCore.OSS](https://github.com/oncemi/OnceMi.AspNetCore.OSS)
  - ASP.NET Core对象储存扩展包，支持Minio自建对象储存、阿里云OSS、腾讯云COS、七牛云Kodo、华为云OBS、百度云BOS、天翼云OOS经典版
  - Z.OSSCore就是这个扩展包的，自己改了一下
- [abp](https://github.com/abpframework/abp)
  - Open Source Web Application Framework for ASP.NET Core
- [MASA framework](https://www.masastack.com/framework)
  - .NET下一代微服务开发框架


还有太多框架未一一列举，感谢开源给予的力量。
