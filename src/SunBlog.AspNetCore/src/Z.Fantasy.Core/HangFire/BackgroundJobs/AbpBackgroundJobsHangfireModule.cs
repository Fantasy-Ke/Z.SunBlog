//using System;
//using System.Linq;
//using Hangfire;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using Volo.Abp.Hangfire;
//using Volo.Abp.Modularity;

//namespace Z.HangFire.BackgroundJobs;

//[DependsOn(
//    typeof(AbpBackgroundJobsAbstractionsModule),
//    typeof(AbpHangfireModule)
//)]
//public class AbpBackgroundJobsHangfireModule : AbpModule
//{
//    public override void ConfigureServices(ServiceConfigurationContext context)
//    {
//        context.Services.AddTransient(serviceProvider =>
//            serviceProvider.GetRequiredService<AbpDashboardOptionsProvider>().Get());
//    }

//    public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
//    {
//        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundJobOptions>>().Value;
//        if (!options.IsJobExecutionEnabled)
//        {
//            var hangfireOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpHangfireOptions>>().Value;
//            hangfireOptions.BackgroundJobServerFactory = CreateOnlyEnqueueJobServer;
//        }
//    }

//    private BackgroundJobServer? CreateOnlyEnqueueJobServer(IServiceProvider serviceProvider)
//    {
//        serviceProvider.GetRequiredService<JobStorage>();
//        return null;
//    }
//}
