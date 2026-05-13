using DntSdkMirror.Models;
using DntSdkMirror.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace DntSdkMirror.Services;

public class ChannelReleasesStoreService(IAppPathService appPathService, ILogger<ChannelReleasesStoreService> logger)
    : IChannelReleasesStoreService
{
    private readonly List<DotNetChannelReleases> _channelReleases = [];

    public void Add(DotNetChannelReleases releases) => _channelReleases.Add(releases);

    public void DeleteAllOldReleases()
    {
        var availableFileInfos = appPathService.GetAllZipFiles();

        foreach (var fileInfo in availableFileInfos)
        {
            var fileReleaseDateInfo = GetReleaseDate(Path.GetFileNameWithoutExtension(fileInfo.Name));

            if (fileReleaseDateInfo.FileReleaseDate is null || (fileReleaseDateInfo.LastChannelReleaseDate.HasValue &&
                                                                fileReleaseDateInfo.LastChannelReleaseDate.Value.Date !=
                                                                fileReleaseDateInfo.FileReleaseDate.Value.Date))

            {
                File.Delete(fileInfo.FullName);

                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug(message: "Deleted an old file: {Path}", fileInfo.Name);
                }
            }
        }
    }

    public (DateTime? FileReleaseDate, DateTime? LastChannelReleaseDate) GetReleaseDate(string fileName)
    {
        foreach (var channelReleases in _channelReleases)
        {
            if (channelReleases.Releases is null)
            {
                continue;
            }

            var lastReleaseDate = channelReleases.Releases.OrderByDescending(release => release.ReleaseDate)
                .First()
                .ReleaseDate;

            foreach (var release in channelReleases.Releases)
            {
                if (HasThisRelease(release.Sdk?.Files, fileName) || HasThisRelease(release.Runtime?.Files, fileName) ||
                    HasThisRelease(release.AspNetCoreRuntime?.Files, fileName) ||
                    HasThisRelease(release.WindowsDesktop?.Files, fileName))
                {
                    return (release.ReleaseDate, lastReleaseDate);
                }
            }
        }

        return (null, null);
    }

    private static bool HasThisRelease(List<FileItem>? files, string fileName)
        => files?.Any(fileItem => string.Equals(Path.GetFileNameWithoutExtension(fileItem.Url), fileName,
            StringComparison.OrdinalIgnoreCase)) == true;
}
