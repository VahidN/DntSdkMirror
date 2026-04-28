namespace DntSdkMirror.Models;

public class SdkInfo
{
    [JsonPropertyName(name: "version")] public string? Version { get; set; }

    [JsonPropertyName(name: "version-display")]
    public string? VersionDisplay { get; set; }

    [JsonPropertyName(name: "runtime-version")]
    public string? RuntimeVersion { get; set; }

    [JsonPropertyName(name: "csharp-version")]
    public string? CSharpVersion { get; set; }

    [JsonPropertyName(name: "fsharp-version")]
    public string? FSharpVersion { get; set; }

    [JsonPropertyName(name: "vb-version")] public string? VbVersion { get; set; }

    [JsonPropertyName(name: "files")] public List<FileItem> Files { get; set; } = [];
}
