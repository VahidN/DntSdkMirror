namespace DntSdkMirror.Models;

public class AppConfig
{
    [Required] public string? OutputFolderName { set; get; }

    [Required] public string? ReleasesIndexUrl { set; get; }

    [Required] public string? ReadmeFileName { set; get; }

    [Required] public string? Owner { set; get; }

    [Required] public string? Repository { set; get; }

    [Required] public string? Branch { set; get; }

    public bool DownloadOneFileEachTime { set; get; }
}
