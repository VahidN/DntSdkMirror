namespace DntSdkMirror.Tests.Factory;

public class FakeHttpMessageHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var content = "";

        if (UrlIsEqualTo(request,
                url: "https://raw.githubusercontent.com/dotnet/core/refs/heads/main/release-notes/releases-index.json"))
        {
            content = await File.ReadAllTextAsync(path: "Resources/releases-index.json", cancellationToken);
        }
        else if (UrlEndsWith(request, text: "releases.json") || UrlEndsWith(request, text: ".gz") ||
                 UrlEndsWith(request, text: ".exe"))
        {
            content = await File.ReadAllTextAsync(path: "Resources/releases.json", cancellationToken);
        }

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(content)
        };
    }

    private static bool UrlIsEqualTo(HttpRequestMessage request, string url)
        => request?.RequestUri?.ToString().Equals(url, StringComparison.OrdinalIgnoreCase) == true;

    private static bool UrlEndsWith(HttpRequestMessage request, string text)
        => request?.RequestUri?.ToString().EndsWith(text, StringComparison.OrdinalIgnoreCase) == true;
}
