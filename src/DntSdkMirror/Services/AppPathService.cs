using DntSdkMirror.Models;
using DntSdkMirror.Services.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DntSdkMirror.Services;

public class AppPathService(
    IHostEnvironment hostEnvironment,
    IOptions<AppConfig> appConfig,
    ILogger<AppPathService> logger) : IAppPathService
{
    public string OutputFolderPath => field ??= GetOutputFolderPath();

    public string RootPath => field ??= GetRootPath();

    public string ReadmeFilePath => field ??= Path.Join(RootPath, appConfig.Value.ReadmeFileName);

    private string GetOutputFolderPath()
    {
        var outPutPath = Path.Join(RootPath, appConfig.Value.OutputFolderName);
        CheckDirExists(outPutPath);

        return outPutPath;
    }

    private static void CheckDirExists(string outputFolderPath)
    {
        if (!Directory.Exists(outputFolderPath))
        {
            Directory.CreateDirectory(outputFolderPath);
        }
    }

    private string GetRootPath()
    {
        var projectFolder = hostEnvironment.ContentRootPath.Split(
            [$"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}"],
            StringSplitOptions.RemoveEmptyEntries)[0];

        var folderNames = projectFolder.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)[..^2];

        var rootPath = string.Join(Path.DirectorySeparatorChar, folderNames);

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug(message: "ContentRootPath: {ContentRootPath}", hostEnvironment.ContentRootPath);
            logger.LogDebug(message: "Using root: {Root}", rootPath);
        }

        return rootPath;
    }
}
