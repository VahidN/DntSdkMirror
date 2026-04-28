namespace DntSdkMirror.IoC;

public static class NamedHttpClient
{
    public const string BaseHttpClient = nameof(BaseHttpClient);

    public static HttpClient CreateBaseHttpClient(this IHttpClientFactory httpClientFactory)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);

        return httpClientFactory.CreateClient(BaseHttpClient);
    }
}
