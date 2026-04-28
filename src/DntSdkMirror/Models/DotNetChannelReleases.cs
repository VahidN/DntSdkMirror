namespace DntSdkMirror.Models;

public class DotNetChannelReleases
{
    [JsonPropertyName(name: "channel-version")]
    public string? ChannelVersion { get; set; }

    [JsonPropertyName(name: "latest-release")]
    public string? LatestRelease { get; set; }

    [JsonPropertyName(name: "latest-release-date")]
    public DateTime? LatestReleaseDate { get; set; }

    [JsonPropertyName(name: "support-phase")]
    public string? SupportPhase { get; set; }

    [JsonPropertyName(name: "releases")] public List<Release>? Releases { get; set; } = [];
}
