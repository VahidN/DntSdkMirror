using DntSdkMirror.Services.Contracts;

namespace DntSdkMirror.Services;

/// <summary>
///     Defines the entry point of the application
/// </summary>
public class AppRunnerService(
    IFetchSDKsService fetchSdKsService,
    IReadmeGeneratorService readmeGeneratorService,
    IChannelReleasesStoreService channelReleasesStoreService) : IAppRunnerService
{
    public async Task<bool> StartAsync(string[] args)
    {
        var success = await fetchSdKsService.StartAsync(args);
        channelReleasesStoreService.DeleteAllOldReleases();
        readmeGeneratorService.UpdateReadmeFile();

        return success;
    }
}
