using DntSdkMirror.IoC;
using DntSdkMirror.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) => services.AddServices(context.Configuration))
    .Build()
    .Services.GetRequiredService<IAppRunnerService>()
    .StartAsync(args);
