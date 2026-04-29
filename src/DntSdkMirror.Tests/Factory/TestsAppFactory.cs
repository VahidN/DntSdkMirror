using DntSdkMirror.IoC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DntSdkMirror.Tests.Factory;

internal static class TestsAppFactory
{
    private static readonly Lazy<IHost> HostProvider = new(GetHost, LazyThreadSafetyMode.ExecutionAndPublication);

    /// <summary>
    ///     A lazy loaded thread-safe singleton
    /// </summary>
    public static IHost Host { get; } = HostProvider.Value;

    public static T GetRequiredService<T>()
        where T : notnull
        => Host.Services.GetRequiredService<T>();

    private static IHost GetHost()
        => Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .UseContentRoot(Path.GetFullPath(AppContext.BaseDirectory))
            .ConfigureServices((context, services) =>
            {
                var httpClientBuilder = services.AddAppServices(context.Configuration);
                services.AddSingleton<FakeHttpMessageHandler>();
                httpClientBuilder.AddHttpMessageHandler<FakeHttpMessageHandler>();
            })
            .Build();
}
