using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Z.Module.Extensions;
using static System.Formats.Asn1.AsnWriter;

namespace Z.Fantasy.Core.AutofacExtensions;

public static class ZAutofacHostBuilderExtensions
{
    public static IHostBuilder UseAutofac(this IHostBuilder hostBuilder)
    {
        var containerBuilder = new ContainerBuilder();

        return hostBuilder.ConfigureServices((_, services) =>
            {
                services.AddObjectAccessor(containerBuilder);
            })
            .UseServiceProviderFactory(new ZAutofacServiceProviderFactory(containerBuilder))
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterBuildCallback(scope =>
                {
                    IOCManager.Current = (IContainer)scope;
                });
            });
    }
}
