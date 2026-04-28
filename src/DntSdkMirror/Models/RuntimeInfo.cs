namespace DntSdkMirror.Models;

public class RuntimeInfo
{
    [JsonPropertyName(name: "version")] public string? Version { get; set; }

    [JsonPropertyName(name: "version-display")]
    public string? VersionDisplay { get; set; }

    [JsonPropertyName(name: "vs-version")] public string? VsVersion { get; set; }

    [JsonPropertyName(name: "vs-mac-version")]
    public string? VsMacVersion { get; set; }

    [JsonPropertyName(name: "files")] public List<FileItem> Files { get; set; } = [];
}
