namespace DntSdkMirror.Utils;

public static class DownloaderServiceExtensions
{
    private const int
        MaxBufferSize =
            0x10000; // 64K. The artificial constraint due to win32 api limitations. Increasing the buffer size beyond 64k will not help in any circumstance, as the underlying SMB protocol does not support buffer lengths beyond 64k.

    public static async Task<bool> DownloadFileAsync(this HttpClient client,
        [StringSyntax(syntax: "Uri")] string url,
        string outputFilePath,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(client);

        var bytesTransferred = 0L;
        var fileMode = FileMode.CreateNew;
        var fileInfo = new FileInfo(outputFilePath);

        if (fileInfo.Exists)
        {
            bytesTransferred = fileInfo.Length;
            fileMode = FileMode.Append;
        }

        if (bytesTransferred > 0)
        {
            client.DefaultRequestHeaders.Range = new RangeHeaderValue(bytesTransferred, to: null);
        }

        var (response, RemoteFileSize) = await ReadResponseHeadersAsync(client, url, cancellationToken);

        if (bytesTransferred > 0 && RemoteFileSize == bytesTransferred)
        {
            return false;
        }

        await using var inputStream = await response.Content.ReadAsStreamAsync(cancellationToken);

        await using var fileStream = outputFilePath.CreateAsyncFileStream(fileMode, FileAccess.Write);

        var supportsRange = response.Headers.AcceptRanges.Contains(value: "bytes", StringComparer.OrdinalIgnoreCase);

        if (!supportsRange && fileStream.Length > 0)
        {
            // Resume is not supported. Starting over.
            fileStream.SetLength(value: 0);
            await fileStream.FlushAsync(cancellationToken);
        }

        await inputStream.ReadInputStreamAsync(fileStream, cancellationToken);

        return true;
    }

    private static async Task<(HttpResponseMessage Response, long RemoteFileSize)> ReadResponseHeadersAsync(
        HttpClient client,
        [StringSyntax(syntax: "Uri")] string url,
        CancellationToken cancellationToken)
    {
        var response = await client.GetAsync(new Uri(url), HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        var RemoteFileSize = response.Content.Headers?.ContentRange?.Length ??
                             response.Content?.Headers?.ContentLength ?? 0;

        return (response, RemoteFileSize);
    }

    private static FileStream CreateAsyncFileStream(this string path, FileMode openOrCreateMode, FileAccess fileAccess)
        => new(path, openOrCreateMode, fileAccess, FileShare.None, MaxBufferSize, useAsync: true);

    private static async Task ReadInputStreamAsync(this Stream inputStream,
        Stream outputStream,
        CancellationToken cancellationToken)
    {
        var buffer = new byte[MaxBufferSize];
        int read;

        while ((read = await inputStream.ReadAsync(buffer.AsMemory(start: 0, buffer.Length), cancellationToken)) > 0 &&
               !cancellationToken.IsCancellationRequested)
        {
            await outputStream.WriteAsync(buffer.AsMemory(start: 0, read), cancellationToken);
        }
    }
}
