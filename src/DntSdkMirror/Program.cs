using DntSdkMirror.IoC;
using DntSdkMirror.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .UseContentRoot(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory)))
    .ConfigureServices((context, services) => services.AddAppServices(context.Configuration))
    .Build()
    .Services.GetRequiredService<IAppRunnerService>()
    .StartAsync(args);
