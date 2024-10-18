using LeanCode.AzureIdentity;
using LeanCode.Logging;
using LeanCode.Startup.MicrosoftDI;
using Microsoft.Extensions.Hosting;
using Serilog.Events;

namespace ExampleApp.Examples;

public class Program
{
    public static Task Main() => CreateWebHostBuilder().Build().RunAsync();

    public static IHostBuilder CreateWebHostBuilder()
    {
        return LeanProgram
            .BuildMinimalHost<Startup>()
            .AddAppConfigurationFromAzureKeyVaultOnNonDevelopmentEnvironment()
            .ConfigureDefaultLogging(
                "ExampleApp.Examples",
                [typeof(Program).Assembly],
                additionalLoggingConfiguration: (context, config) =>
                {
                    // Silence noisy libraries
                    config.MinimumLevel.Override("Azure.Messaging.ServiceBus", LogEventLevel.Warning);
                    config.MinimumLevel.Override("Azure.Identity", LogEventLevel.Warning);
                }
            );
    }
}
