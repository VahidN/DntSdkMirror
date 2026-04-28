namespace DntSdkMirror.Services.Contracts;

public interface IAppRunnerService
{
    Task<bool> StartAsync(string[] args);
}
