using Z.Fantasy.Core.Serilog;
using Z.Foundation.Core.AutofacExtensions;
using Z.Foundation.Core.Helper;
using Z.Module.Extensions;
using Z.SunBlog.Host;
using Z.SunBlog.Host.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

//builder.Logging.ClearProviders();

builder.Services.AddSingleton(new AppSettings(builder));

builder.Host.AddSerilogSetup();

builder.Host.UseAutofac();
//基础服务注入
builder.Services.AddCoreServices();

builder.Services.AddApplication<SunBlogHostModule>();

var app = builder.Build();

//基础管道加入
app.AddUseCore();

await app.InitApplicationAsync();

app.Run();


