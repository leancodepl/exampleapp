using ExampleApp.Examples.Configuration;
using LeanCode.AzureIdentity;
using LeanCode.Logging;
using LeanCode.Startup.MicrosoftDI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
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
            .ConfigureAppConfiguration(
                (host, opts) =>
                {
                    if (host.HostingEnvironment.IsDevelopment())
                    {
                        opts.AddJsonFile("appsettings.local.json", optional: true);
                    }
                }
            )
            .ConfigureDefaultLogging(
                "ExampleApp.Examples",
                [typeof(Program).Assembly],
                additionalLoggingConfiguration: (ctx, config) =>
                {
                    // Silence noisy libraries
                    config.MinimumLevel.Override("Azure.Messaging.ServiceBus", LogEventLevel.Warning);
                    config.MinimumLevel.Override("Azure.Identity", LogEventLevel.Warning);

                    if (
                        ctx.HostingEnvironment.IsDevelopment()
                        && AppConfig.Telemetry.OtlpEndpoint(ctx.Configuration) is { } otlp
                    )
                    {
                        config.WriteTo.OpenTelemetry(o =>
                        {
                            o.Endpoint = otlp;
                            o.ResourceAttributes = new Dictionary<string, object>
                            {
                                ["service.name"] = "ExampleApp.Examples",
                                ["service.instance.id"] = Environment.MachineName,
                            };
                        });
                    }
                }
            );
    }
}
