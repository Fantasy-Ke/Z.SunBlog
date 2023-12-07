using Z.Ddd.Common.Helper;
using Z.Ddd.Common.Serilog;
using Z.EventBus.Extensions;
using Z.Module.Extensions;
using Z.SunBlog.Host;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var configuration = builder.Configuration;

//int.TryParse(configuration["App:WebHostPort"], out var port);

//port = port == default ? 80 : port;

//builder.WebHost.UseKestrel(op => op.ListenAnyIP(port));

builder.Services.AddHttpContextAccessor();

builder.Logging.ClearProviders();

builder.Services.AddSingleton(new AppSettings(builder));

builder.Host.AddSerilogSetup();

builder.Services.AddApplication<SunBlogHostModule>();


var app = builder.Build();

await app.InitApplicationAsync();

app.Run();
