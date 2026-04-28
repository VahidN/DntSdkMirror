namespace DntSdkMirror.Services.Contracts;

public interface IFetchSDKsService
{
    Task<bool> StartAsync(string[] args);
}
