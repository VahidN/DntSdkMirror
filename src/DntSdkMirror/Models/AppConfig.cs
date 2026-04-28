namespace DntSdkMirror.Models;

public class AppConfig
{
    public required string OutputFolderName { set; get; }

    public required string ReleasesIndexUrl { set; get; }

    public required string ReadmeFileName { set; get; }

    public required string Owner { set; get; }

    public required string Repository { set; get; }

    public required string Branch { set; get; }
}
