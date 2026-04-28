using DntSdkMirror.IoC;
using DntSdkMirror.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                              throw new InvalidOperationException(message: "dirName is null"));

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) => services.AddServices(context.Configuration))
    .Build()
    .Services.GetRequiredService<IAppRunnerService>()
    .StartAsync(args);
