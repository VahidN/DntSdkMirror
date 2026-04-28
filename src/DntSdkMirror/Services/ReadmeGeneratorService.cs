using DntSdkMirror.Models;
using DntSdkMirror.Services.Contracts;
using DntSdkMirror.Utils;
using Microsoft.Extensions.Options;

namespace DntSdkMirror.Services;

public class ReadmeGeneratorService(IAppPathService appPathService, IOptions<AppConfig> appConfig)
    : IReadmeGeneratorService
{
    private const string TableSeparator = "<!---->";

    public void UpdateReadmeFile()
    {
        if (!File.Exists(appPathService.ReadmeFilePath))
        {
            return;
        }

        var readmeFileContent = File.ReadAllText(appPathService.ReadmeFilePath);
        var mainContent = readmeFileContent.Split(TableSeparator, StringSplitOptions.RemoveEmptyEntries)[0];

        ICollection<ICollection<string>> rows = [];
        var sdksDirInfo = new DirectoryInfo(appPathService.OutputFolderPath);

        foreach (var fileInfo in sdksDirInfo.GetFiles(searchPattern: "*.*", SearchOption.AllDirectories))
        {
            rows.Add([
                fileInfo.Directory?.Name ?? "", GetDownloadLink(fileInfo), fileInfo.Length.ToFormattedFileSize()
            ]);
        }

        var sdksTableContent = MarkdownTableGenerator.GenerateMarkdownTable(["کانال", "فایل", "حجم"], rows);

        File.WriteAllText(appPathService.ReadmeFilePath, $"{mainContent}{TableSeparator}\n{sdksTableContent}");
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
