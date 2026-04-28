using System.Net.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace DntSdkMirror.IoC;

public static class ConfigureHttpClient
{
    public static IHttpClientBuilder AddHttpClient(this IServiceCollection services)
    {
        var builder = services.AddHttpClient(NamedHttpClient.BaseHttpClient, AddDefaultSettings)
            .ConfigurePrimaryHttpMessageHandler(() => CreateScrapingHandler(allowAutoRedirect: true))
            .AddPolicyHandler(GetResiliencePolicy());

        services.RemoveAll<IHttpMessageHandlerBuilderFilter>(); // Remove logging of the HttpClient

        return builder;
    }

    private static void AddDefaultSettings(HttpClient client)
    {
        client.DefaultRequestHeaders.Clear();

        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            input:
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.6099.216 Safari/537.36");

        client.DefaultRequestHeaders.Accept.ParseAdd(
            input: "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

        client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(input: "en-US,en;q=0.9");
        client.DefaultRequestHeaders.AcceptEncoding.ParseAdd(input: "gzip, deflate");

        client.DefaultRequestHeaders.ConnectionClose = false;
        client.DefaultRequestHeaders.ExpectContinue = false;

        client.DefaultRequestVersion = new Version(major: 2, minor: 0); // Prefer HTTP/2
        client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
    }

    private static SocketsHttpHandler CreateScrapingHandler(bool allowAutoRedirect)
        => new()
        {
            UseProxy = false,
            AutomaticDecompression = DecompressionMethods.All,
            PooledConnectionLifetime = TimeSpan.FromMinutes(value: 10),
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(value: 2),
            MaxConnectionsPerServer = 100,
            ConnectTimeout = TimeSpan.FromSeconds(value: 15),
            KeepAlivePingPolicy = HttpKeepAlivePingPolicy.WithActiveRequests,
            KeepAlivePingDelay = TimeSpan.FromMinutes(value: 2),
            KeepAlivePingTimeout = TimeSpan.FromSeconds(value: 45),
            ResponseDrainTimeout = TimeSpan.FromMinutes(value: 5),
            UseCookies = true,
            CookieContainer = new CookieContainer(),
            AllowAutoRedirect = allowAutoRedirect,
            MaxAutomaticRedirections = 5 // Limit to prevent infinite loops
        };

    private static AsyncPolicy<HttpResponseMessage> GetResiliencePolicy()
    {
        // ۱. پالیسی تلاش مجدد: اگر خطا داد یا تایم‌اوت شد، دوباره تلاش کن
        var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError() // خطاهای ۵xx و ۴۰۸
            .Or<TaskCanceledException>() // network stall
            .Or<TimeoutRejectedException>() // اگر پالیسی تایم‌اوت فعال شد
            .OrInner<SocketException>() // خطاهای شبکه در لایه‌های پایین
            .OrResult(r => (int)r.StatusCode >= 500 || r.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(retryCount: 3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(x: 2, retryAttempt))); // Exponential Backoff: 2s, 4s, 8s

        // ۲. پالیسی تایم‌اوت: برای هر درخواست جداگانه
        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(
            TimeSpan.FromSeconds(value: 45), TimeoutStrategy.Optimistic);

        // نکته مهم: ریترای دورِ تایم‌اوت را می‌گیرد (Retry wraps Timeout)
        return Policy.WrapAsync(retryPolicy, timeoutPolicy);
    }
}
