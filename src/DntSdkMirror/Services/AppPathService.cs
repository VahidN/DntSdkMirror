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
        var root = Path.GetFullPath(hostEnvironment.ContentRootPath);

        var projectFolder = Path.GetFullPath(Path.Combine(
            root.Split([$"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}"],
                StringSplitOptions.RemoveEmptyEntries)[0], path2: "..", path3: ".."));

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug(message: "Using root: {Root}", root);
            logger.LogDebug(message: "Using projectFolder: {ProjectFolder}", projectFolder);
        }

        return projectFolder;
    }
}
