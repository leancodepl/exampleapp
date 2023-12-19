using LeanCode.AzureIdentity;
using LeanCode.Logging;
using LeanCode.Startup.MicrosoftDI;
using Microsoft.Extensions.Hosting;

namespace ExampleApp.Examples.Api;

public class Program
{
    public static Task Main() => CreateWebHostBuilder().Build().RunAsync();

    public static IHostBuilder CreateWebHostBuilder()
    {
        return LeanProgram
            .BuildMinimalHost<Startup>()
            .AddAppConfigurationFromAzureKeyVaultOnNonDevelopmentEnvironment()
            .ConfigureDefaultLogging("ExampleApp.Examples.Api", [typeof(Program).Assembly]);
    }
}
