namespace DntSdkMirror.Models;

public class ReleaseInfo
{
    [JsonPropertyName(name: "channel-version")]
    public string? ChannelVersion { get; set; }

    [JsonPropertyName(name: "latest-release")]
    public string? LatestRelease { get; set; }

    [JsonPropertyName(name: "latest-release-date")]
    public DateTime? LatestReleaseDate { get; set; }

    [JsonPropertyName(name: "security")] public bool Security { get; set; }

    [JsonPropertyName(name: "latest-runtime")]
    public string? LatestRuntime { get; set; }

    [JsonPropertyName(name: "latest-sdk")] public string? LatestSdk { get; set; }

    [JsonPropertyName(name: "product")] public string? Product { get; set; }

    [JsonPropertyName(name: "support-phase")]
    public string? SupportPhase { get; set; }

    [JsonPropertyName(name: "release-type")]
    public string? ReleaseType { get; set; }

    [JsonPropertyName(name: "eol-date")] public DateTime? EolDate { get; set; }

    [JsonPropertyName(name: "releases.json")]
    public string? ReleasesJsonUrl { get; set; }

    [JsonPropertyName(name: "supported-os.json")]
    public string? SupportedOsUrl { get; set; }
}
