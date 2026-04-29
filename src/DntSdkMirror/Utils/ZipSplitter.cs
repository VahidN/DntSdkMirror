using Microsoft.Extensions.Logging;

namespace DntSdkMirror.Utils;

public static class ZipSplitter
{
    public static IList<string>? SplitZip(string filePath,
        int partSizeMB,
        string outputDirectory,
        bool overwriteExistingFiles,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        if (!File.Exists(filePath))
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(message: "`{File}` doesn't exists.", filePath);
            }

            return null;
        }

        // پاکسازی فایل‌های قبلی با همان نام (برای جلوگیری از تداخل)
        var (existingFiles, outputZipPath) = GetExistingZipFiles(filePath, outputDirectory);

        if (existingFiles.Count > 0)
        {
            if (overwriteExistingFiles)
            {
                foreach (var file in existingFiles)
                {
                    try { File.Delete(file); }
                    catch
                    {
                        /* نادیده گرفتن خطای حذف */
                    }
                }
            }
            else
            {
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug(message: "The zip file already exists.");
                }

                return null;
            }
        }

        List<string> args =
        [
            "-s", string.Create(CultureInfo.InvariantCulture, $"{partSizeMB}m"), "-q", outputZipPath, filePath
        ];

        var processStartInfo = new ProcessStartInfo
        {
            FileName = "zip",
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var arg in args)
        {
            processStartInfo.ArgumentList.Add(arg);
        }

        using (var process = new Process())
        {
            process.StartInfo = processStartInfo;
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug(message: "`Error processing zip file: {Error}", error);
                }

                return null;
            }
        }

        var (createdFiles, _) = GetExistingZipFiles(filePath, outputDirectory);

        if (logger.IsEnabled(LogLevel.Debug))
        {
            foreach (var createdFile in createdFiles)
            {
                logger.LogDebug(message: "Created zip file: {File}", createdFile);
            }
        }

        return createdFiles.Count == 0 ? null : createdFiles;
    }

    public static (List<string> Parts, string OutputZipPath) GetExistingZipFiles(string filePath,
        string outputDirectory)
    {
        var fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
        var outputZipName = $"{fileNameWithoutExt}.zip";
        var outputZipPath = Path.Combine(outputDirectory, outputZipName);
        var outputBase = Path.Combine(outputDirectory, fileNameWithoutExt);

        return (
            Directory.GetFiles(outputDirectory, $"{fileNameWithoutExt}.*")
                .Where(f => string.Equals(f, outputZipPath, StringComparison.OrdinalIgnoreCase) ||
                            f.StartsWith($"{outputBase}.z", StringComparison.OrdinalIgnoreCase))
                .OrderBy(f => f, StringComparer.Ordinal)
                .ToList(), outputZipPath);
    }
}
