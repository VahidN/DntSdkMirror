using System.Text;
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

        var numericComparer = StringComparer.Create(CultureInfo.InvariantCulture, CompareOptions.NumericOrdering);

        var markDown = new StringBuilder();

        foreach (var fileGroupInfo in new DirectoryInfo(appPathService.OutputFolderPath)
                     .GetFiles(searchPattern: "*.*", SearchOption.AllDirectories)
                     .Where(fileInfo => fileInfo.Name.StartsWith($"{Path.GetFileNameWithoutExtension(fileInfo.Name)}.z",
                         StringComparison.OrdinalIgnoreCase))
                     .Select(fileInfo => new ZipFileItem(fileInfo.Directory?.Name ?? "", GetDownloadLink(fileInfo),
                         fileInfo.Length.ToFormattedFileSize(), fileInfo.LastWriteTimeUtc))
                     .GroupBy(zipFileItem => zipFileItem.Channel))
        {
            ICollection<ICollection<string>> rows = [];

            foreach (var fileInfo in fileGroupInfo.OrderByDescending(zipFileItem => zipFileItem.LastWriteTime)
                         .ThenByDescending(zipFileItem => zipFileItem.FileName, numericComparer))
            {
                rows.Add([fileInfo.FileName, fileInfo.SizeMB, GetPersianDay(fileInfo.LastWriteTime)]);
            }

            markDown.AppendLine()
                .Append(value: "**")
                .Append(value: "کانال دات‌نت ")
                .Append(fileGroupInfo.Key)
                .Append(value: ':')
                .AppendLine(value: "**")
                .AppendLine(MarkdownTableGenerator.GenerateMarkdownTable(["فایل", "حجم", "تاریخ"], rows))
                .AppendLine()
                .AppendLine();
        }

        var sdksTableContent = markDown.ToString();

        if (string.IsNullOrWhiteSpace(sdksTableContent))
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(message: "Skipped updating `{File}`. There's nothing new to add.", readmeFilePath);
            }

            return;
        }

        var readmeFileContent = File.ReadAllText(readmeFilePath);
        var mainContent = readmeFileContent.Split(TableSeparator, StringSplitOptions.RemoveEmptyEntries)[0];
        File.WriteAllText(readmeFilePath, $"{mainContent}{TableSeparator}\n{sdksTableContent}");

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug(message: "Finished updating `{File}`.", readmeFilePath);
        }
    }

    private static string GetPersianDay(DateTime lastWriteTime)
        => string.Create(CultureInfo.InvariantCulture,
            $"{new PersianCalendar().GetYear(lastWriteTime)}/{new PersianCalendar().GetMonth(lastWriteTime):00}/{new PersianCalendar().GetDayOfMonth(lastWriteTime):00}");

    private string GetDownloadLink(FileInfo fileInfo)
    {
        var config = appConfig.Value;

        var relativeFilePath =
            $"{config.OutputFolderName}{fileInfo.FullName.Replace(appPathService.OutputFolderPath, newValue: "",
                StringComparison.OrdinalIgnoreCase)}".Replace(oldChar: '\\', newChar: '/');

        return
            $"[{fileInfo.Name}](https://github.com/{config.Owner}/{config.Repository}/raw/refs/heads/{config.Branch}/{relativeFilePath})";
    }
}
