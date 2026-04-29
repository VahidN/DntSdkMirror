using DntSdkMirror.Models;
using DntSdkMirror.Services.Contracts;
using DntSdkMirror.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DntSdkMirror.Services;

public class ReadmeGeneratorService(
    IAppPathService appPathService,
    IOptions<AppConfig> appConfig,
    ILogger<ReadmeGeneratorService> logger) : IReadmeGeneratorService
{
    private const string TableSeparator = "<!---->";

    public void UpdateReadmeFile()
    {
        var readmeFilePath = appPathService.ReadmeFilePath;

        if (!File.Exists(readmeFilePath))
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(message: "`{File}` doesn't exists.", readmeFilePath);
            }

            return;
        }

        var readmeFileContent = File.ReadAllText(readmeFilePath);
        var mainContent = readmeFileContent.Split(TableSeparator, StringSplitOptions.RemoveEmptyEntries)[0];

        ICollection<ICollection<string>> rows = [];
        var sdksDirInfo = new DirectoryInfo(appPathService.OutputFolderPath);

        foreach (var fileInfo in sdksDirInfo.GetFiles(searchPattern: "*.exe", SearchOption.AllDirectories))
        {
            rows.Add([
                fileInfo.Directory?.Name ?? "", GetDownloadLink(fileInfo), fileInfo.Length.ToFormattedFileSize()
            ]);
        }

        var sdksTableContent = MarkdownTableGenerator.GenerateMarkdownTable(["کانال", "فایل", "حجم"], rows);

        if (string.IsNullOrWhiteSpace(sdksTableContent))
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(message: "Skipped updating `{File}`. There's nothing new to add.", readmeFilePath);
            }

            return;
        }

        File.WriteAllText(readmeFilePath, $"{mainContent}{TableSeparator}\n{sdksTableContent}");

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug(message: "Finished updating `{File}`.", readmeFilePath);
        }
    }

    private string GetDownloadLink(FileInfo fileInfo)
    {
        var config = appConfig.Value;

        var relativeFilePath =
            $"{config.OutputFolderName}{fileInfo.FullName.Replace(appPathService.OutputFolderPath, newValue: "",
                StringComparison.OrdinalIgnoreCase)}".Replace(oldChar: '\\', newChar: '/');

        return
            $"[{fileInfo.Name}](https://raw.githubusercontent.com/{config.Owner}/{config.Repository}/{config.Branch}/{relativeFilePath})";
    }
}
