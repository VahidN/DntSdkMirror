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
    private const int MaxPartSizeMB = 45;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
    };

    public async Task<bool> StartAsync(string[] args)
    {
        if (!ZipSplitter.IsZipInstalled())
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(message: "`zip` app is not installed.");
            }

            return false;
        }

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

            var channelData = await GetChannelReleasesAsync(releaseIndex.ReleasesJsonUrl, releaseIndex.ChannelVersion);
            var lastRelease = channelData?.Releases?.OrderByDescending(release => release.ReleaseDate).FirstOrDefault();

            var channelVersion = channelData?.ChannelVersion;
            await DownloadReleaseFilesAsync(lastRelease?.Runtime?.Files, channelVersion);
            await DownloadReleaseFilesAsync(lastRelease?.Sdk?.Files, channelVersion);
            await DownloadReleaseFilesAsync(lastRelease?.AspNetCoreRuntime?.Files, channelVersion);
            await DownloadReleaseFilesAsync(lastRelease?.WindowsDesktop?.Files, channelVersion);

            if (appConfig.Value.DownloadOneFileEachTime)
            {
                return true;
            }
        }

        return true;
    }

    private async Task DownloadReleaseFilesAsync(List<FileItem>? files, string? channelVersion)
    {
        if (files is null)
        {
            return;
        }

        foreach (var fileUrl in files.Select(fileItem => fileItem.Url)
                     .Where(fileUrl => !string.IsNullOrWhiteSpace(fileUrl)))
        {
            var fileName = Path.GetFileName(fileUrl)!;

            if (ShouldIgnoreReleaseFile(fileName))
            {
                continue;
            }

            var (outputFilePath, outputDirectory) = GetOutputFilePath(fileName, channelVersion);

            if (IsAlreadyDownloaded(fileName, outputDirectory))
            {
                continue;
            }

            if (await DownloadFileAsync(fileUrl!, outputFilePath))
            {
                var zipFiles = ZipSplitter.SplitZip(outputFilePath, MaxPartSizeMB, outputDirectory,
                    overwriteExistingFiles: false, logger);

                if (zipFiles?.Count > 0)
                {
                    File.Delete(outputFilePath);
                }
            }
        }
    }

    private static bool IsAlreadyDownloaded(string fileName, string outputDirectory)
        => ZipSplitter.GetExistingZipFiles(fileName, outputDirectory).Parts.Count > 0;

    private async Task<bool> DownloadFileAsync(string fileUrl, string outputFilePath)
    {
        using var httpClient = httpClientFactory.CreateBaseHttpClient();
        var success = await httpClient.DownloadFileAsync(fileUrl, outputFilePath, logger);

        if (success && logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug(message: "Finished downloading `{OutputFilePath}`. Size: {Size}", outputFilePath,
                new FileInfo(outputFilePath).Length.ToFormattedFileSize());
        }

        return success;
    }

    private (string OutputFilePath, string ChannelFolderPath) GetOutputFilePath(string fileName, string? channelVersion)
    {
        channelVersion ??= "latest";
        var channelFolderPath = Path.Join(appPathService.OutputFolderPath, channelVersion);

        if (!Directory.Exists(channelFolderPath))
        {
            Directory.CreateDirectory(channelFolderPath);
        }

        var outputFilePath = Path.Join(channelFolderPath, fileName);

        return (outputFilePath, channelFolderPath);
    }

    private static bool ShouldIgnoreReleaseFile(string? fileName)
        => !string.IsNullOrWhiteSpace(fileName) &&
           (!fileName.EndsWith(value: ".exe", StringComparison.OrdinalIgnoreCase) ||
            fileName.Contains(value: "arm", StringComparison.OrdinalIgnoreCase) ||
            fileName.Contains(value: "osx", StringComparison.OrdinalIgnoreCase) ||
            fileName.Contains(value: "x86", StringComparison.OrdinalIgnoreCase) ||
            fileName.Contains(value: "musl", StringComparison.OrdinalIgnoreCase));

    private static bool HasNotActiveSupport(ReleaseInfo releaseIndex)
        => releaseIndex.SupportPhase?.Equals(value: "active", StringComparison.OrdinalIgnoreCase) != true;

    private async Task<DotNetChannelReleases?> GetChannelReleasesAsync(string? url, string? channelVersion)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        using var httpClient = httpClientFactory.CreateBaseHttpClient();
        var releasesJsonUrlJsonString = await httpClient.GetStringAsync(url);

        if (string.IsNullOrWhiteSpace(releasesJsonUrlJsonString))
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(message: "Failed to download `{Url}`.", url);
            }

            return null;
        }

        await File.WriteAllTextAsync(
            Path.Join(appPathService.OutputFolderPath, $"{channelVersion}-{Path.GetFileName(url)}"),
            releasesJsonUrlJsonString);

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
        var indexUrl = appConfig.Value.ReleasesIndexUrl;

        if (string.IsNullOrWhiteSpace(indexUrl))
        {
            throw new InvalidOperationException(
                message: "`ReleasesIndexUrl` of `appsettings.json` IsNullOrWhiteSpace.");
        }

        var releasesIndexUrlJsonString = await httpClient.GetStringAsync(indexUrl);

        if (string.IsNullOrWhiteSpace(releasesIndexUrlJsonString))
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(message: "Failed to download `{Url}`.", indexUrl);
            }

            return null;
        }

        await File.WriteAllTextAsync(Path.Join(appPathService.OutputFolderPath, Path.GetFileName(indexUrl)),
            releasesIndexUrlJsonString);

        var index = JsonSerializer.Deserialize<DotNetReleaseIndex>(releasesIndexUrlJsonString, JsonSerializerOptions);

        if (index?.ReleasesIndex is null)
        {
            logger.LogError(message: "Failed to deserialize data: {ReleasesIndexUrlJsonString}",
                releasesIndexUrlJsonString);
        }

        return index;
    }
}
