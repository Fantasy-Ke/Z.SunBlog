using Z.Fantasy.Core.AutofacExtensions;
using Z.Fantasy.Core.Helper;
using Z.Fantasy.Core.Serilog;
using Z.Module.Extensions;
using Z.SunBlog.Host;
using Z.SunBlog.Host.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Logging.ClearProviders();

builder.Services.AddSingleton(new AppSettings(builder));

builder.Host.AddSerilogSetup();
//��������ע��
builder.Services.AddCoreServices();

builder.Services.AddApplication<SunBlogHostModule>();

builder.Host.UseAutofac();

var app = builder.Build();

//�����ܵ�����
app.AddUseCore();

await app.InitApplicationAsync();

app.Run();


