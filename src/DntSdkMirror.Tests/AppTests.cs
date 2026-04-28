using DntSdkMirror.Services.Contracts;
using DntSdkMirror.Tests.Factory;

namespace DntSdkMirror.Tests;

[TestClass]
public class AppTests
{
    [TestMethod]
    public async Task TestAppRunnerServiceWorksAsync()
    {
        var appRunnerService = TestsAppFactory.GetRequiredService<IAppRunnerService>();
        var result = await appRunnerService.StartAsync([]);

        Assert.IsTrue(result);
    }
}
