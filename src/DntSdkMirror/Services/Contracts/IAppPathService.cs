namespace DntSdkMirror.Services.Contracts;

public interface IAppPathService
{
    string OutputFolderPath { get; }

    string RootPath { get; }

    string ReadmeFilePath { get; }
}
