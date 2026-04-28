namespace DntSdkMirror.Models;

public class Release
{
    [JsonPropertyName("release-date")]
    public DateTime ReleaseDate { get; set; }

    [JsonPropertyName("release-version")]
    public string? ReleaseVersion { get; set; }

    [JsonPropertyName("security")]
    public bool Security { get; set; }

    [JsonPropertyName("release-notes")]
    public string? ReleaseNotesUrl { get; set; }

    [JsonPropertyName("runtime")]
    public RuntimeInfo? Runtime { get; set; }

    [JsonPropertyName("sdk")]
    public SdkInfo? Sdk { get; set; }

    [JsonPropertyName("aspnetcore-runtime")]
    public AspNetCoreRuntime? AspNetCoreRuntime { get; set; }

    [JsonPropertyName("windowsdesktop")]
    public WindowsDesktopRuntime? WindowsDesktop { get; set; }
}
