using Z.Ddd.Common.Helper;
using Z.Ddd.Common.Serilog;
using Z.Module.Extensions;
using Z.SunBlog.Host;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Logging.ClearProviders();

builder.Services.AddSingleton(new AppSettings(builder));

builder.Host.AddSerilogSetup();

builder.Services.AddApplication<SunBlogHostModule>();


var app = builder.Build();

await app.InitApplicationAsync();

app.Run();
