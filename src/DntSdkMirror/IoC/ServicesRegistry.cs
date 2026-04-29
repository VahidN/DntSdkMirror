using DntSdkMirror.Models;
using DntSdkMirror.Services;
using DntSdkMirror.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DntSdkMirror.IoC;

public static class ServicesRegistry
{
    public static IHttpClientBuilder AddAppServices(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.Configure<AppConfig>(options => configuration.GetSection(nameof(AppConfig)).Bind(options))
            .AddOptionsWithValidateOnStart<AppConfig>();

        serviceCollection.TryAddSingleton<IFetchSDKsService, FetchSDKsService>();
        serviceCollection.TryAddSingleton<IAppPathService, AppPathService>();
        serviceCollection.TryAddSingleton<IAppRunnerService, AppRunnerService>();
        serviceCollection.TryAddSingleton<IReadmeGeneratorService, ReadmeGeneratorService>();

        var httpClientBuilder = serviceCollection.AddHttpClient();

        return httpClientBuilder;
    }
}
