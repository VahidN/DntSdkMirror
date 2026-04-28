using DntSdkMirror.IoC;
using DntSdkMirror.Models;
using DntSdkMirror.Services.Contracts;
using DntSdkMirror.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DntSdkMirror.Services;

public class FetchSDKsService(
    IHttpClientFactory httpClientFactory,
    IOptions<AppConfig> appConfig,
    IAppPathService appPathService,
    ILogger<FetchSDKsService> logger) : IFetchSDKsService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
    };

    public async Task<bool> StartAsync(string[] args)
    {
        var index = await GetReleasesIndexAsync();

        if (index?.ReleasesIndex is null)
        {
            return false;
        }

        foreach (var releaseIndex in index.ReleasesIndex)
        {
            if (HasNotActiveSupport(releaseIndex))
            {
                continue;
            }

            var channelData = await GetChannelReleasesAsync(releaseIndex.ReleasesJsonUrl);

            var lastRelease = channelData?.Releases?.OrderByDescending(release => release.ReleaseDate).FirstOrDefault();

            if (lastRelease?.Sdk?.Files is null)
            {
                continue;
            }

            foreach (var file in lastRelease.Sdk.Files.Where(file => !string.IsNullOrWhiteSpace(file.Url)))
            {
                await DownloadFileAsync(file.Url, channelData?.ChannelVersion);
            }
        }

        return true;
    }

    private async Task DownloadFileAsync(string? fileUrl, string? channelVersion)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return;
        }

        var fileName = Path.GetFileName(fileUrl);

        if (!fileName.EndsWith(value: ".exe", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        channelVersion ??= "latest";
        var channelFolderPath = Path.Join(appPathService.OutputFolderPath, channelVersion);

        if (!Directory.Exists(channelFolderPath))
        {
            Directory.CreateDirectory(channelFolderPath);
        }

        var outputFilePath = Path.Join(channelFolderPath, fileName);

        using var httpClient = httpClientFactory.CreateBaseHttpClient();
        var success = await httpClient.DownloadFileAsync(fileUrl, outputFilePath);

        if (success && logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug(message: "Finished downloading `{OutputFilePath}`.", outputFilePath);
        }
    }

    private static bool HasNotActiveSupport(ReleaseInfo releaseIndex)
        => releaseIndex.SupportPhase is null ||
           (!releaseIndex.SupportPhase.Equals(value: "active", StringComparison.OrdinalIgnoreCase) &&
            !releaseIndex.SupportPhase.Equals(value: "preview", StringComparison.OrdinalIgnoreCase));

    private async Task<DotNetChannelReleases?> GetChannelReleasesAsync(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        using var httpClient = httpClientFactory.CreateBaseHttpClient();
        var releasesJsonUrlJsonString = await httpClient.GetStringAsync(url);

        if (string.IsNullOrWhiteSpace(releasesJsonUrlJsonString))
        {
            return null;
        }

        var channelData =
            JsonSerializer.Deserialize<DotNetChannelReleases>(releasesJsonUrlJsonString, JsonSerializerOptions);

        if (channelData?.Releases is null)
        {
            logger.LogError(message: "Failed to deserialize data: {ReleasesJsonUrlJsonString}",
                releasesJsonUrlJsonString);
        }

        return channelData;
    }

    private async Task<DotNetReleaseIndex?> GetReleasesIndexAsync()
    {
        using var httpClient = httpClientFactory.CreateBaseHttpClient();
        var releasesIndexUrlJsonString = await httpClient.GetStringAsync(appConfig.Value.ReleasesIndexUrl);

        if (string.IsNullOrWhiteSpace(releasesIndexUrlJsonString))
        {
            return null;
        }

        var index = JsonSerializer.Deserialize<DotNetReleaseIndex>(releasesIndexUrlJsonString, JsonSerializerOptions);

        if (index?.ReleasesIndex is null)
        {
            logger.LogError(message: "Failed to deserialize data: {ReleasesIndexUrlJsonString}",
                releasesIndexUrlJsonString);
        }

        return index;
    }
}
