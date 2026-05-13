using DntSdkMirror.Models;

namespace DntSdkMirror.Services.Contracts;

public interface IChannelReleasesStoreService
{
    void Add(DotNetChannelReleases releases);

    (DateTime? FileReleaseDate, DateTime? LastChannelReleaseDate) GetReleaseDate(string fileName);

    void DeleteAllOldReleases();
}
