namespace DntSdkMirror.Models;

public class DotNetReleaseIndex
{
    [JsonPropertyName(name: "$schema")] public string? Schema { get; set; }

    [JsonPropertyName(name: "releases-index")]
    public List<ReleaseInfo> ReleasesIndex { get; set; } = [];
}
