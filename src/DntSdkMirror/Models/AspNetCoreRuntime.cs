namespace DntSdkMirror.Models;

public class AspNetCoreRuntime
{
    [JsonPropertyName(name: "version")] public string? Version { get; set; }

    [JsonPropertyName(name: "version-display")]
    public string? VersionDisplay { get; set; }

    [JsonPropertyName(name: "files")] public List<FileItem> Files { get; set; } = [];
}
