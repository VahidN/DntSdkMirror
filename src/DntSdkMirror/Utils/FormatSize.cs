namespace DntSdkMirror.Utils;

/// <summary>
///     File Size Formatter
/// </summary>
public static class FormatSize
{
    private static readonly string[] SizeSuffixes = ["B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"];

    /// <summary>
    ///     Returns the file's size in KB/MB format
    /// </summary>
    public static string ToFormattedFileSize(this int size) => ((long)size).ToFormattedFileSize();

    /// <summary>
    ///     Returns the file's size in KB/MB format
    /// </summary>
    public static string ToFormattedFileSize(this long size)
    {
        const string FormatTemplate = "{0}{1:0.#} {2}";

        if (size == 0)
        {
            return string.Format(CultureInfo.InvariantCulture, FormatTemplate, arg0: null, arg1: 0, SizeSuffixes[0]);
        }

        var absSize = Math.Abs((double)size);
        var fpPower = Math.Log(absSize, newBase: 1000);
        var intPower = (int)fpPower;
        var iUnit = intPower >= SizeSuffixes.Length ? SizeSuffixes.Length - 1 : intPower;
        var normSize = absSize / Math.Pow(x: 1000, iUnit);

        return string.Format(CultureInfo.InvariantCulture, FormatTemplate, size < 0 ? "-" : null, normSize,
            SizeSuffixes[iUnit]);
    }
}
