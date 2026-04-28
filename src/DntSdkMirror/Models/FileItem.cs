namespace DntSdkMirror.Models;

public class FileItem
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("rid")]
    public string? Rid { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("hash")]
    public string? Hash { get; set; }

    [JsonPropertyName("akams")]
    public string? Akams { get; set; } // برخی فایل‌ها (مانند dotnet-hosting-win.exe) این فیلد را دارند
}
