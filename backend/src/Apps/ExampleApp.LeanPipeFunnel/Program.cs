using System.Globalization;
using ExampleApp.LeanPipeFunnel;
using ExampleApp.LeanPipeFunnel.Handlers;
using LeanCode.AzureIdentity;
using LeanCode.Logging;
using LeanCode.OpenTelemetry;
using LeanCode.Pipe;
using LeanCode.Pipe.Funnel.Instance;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var appBuilder = WebApplication.CreateBuilder(args);
var hostBuilder = appBuilder.Host;

hostBuilder
    .AddAppConfigurationFromAzureKeyVaultOnNonDevelopmentEnvironment()
    .ConfigureDefaultLogging("ExampleApp.LeanPipeFunnel", [typeof(Program).Assembly]);

var hostEnv = appBuilder.Environment;
var services = appBuilder.Services;
var config = appBuilder.Configuration;

if (!Config.LeanPipe.EnableLeanPipeFunnel(config))
{
    throw new InvalidOperationException("LeanPipe Funnel must be enabled in the configuration to be used.");
}

services.AddLeanPipeFunnel();

services
    .AddAuthentication()
    .AddKratos(options =>
    {
        options.NameClaimType = Auth.KnownClaims.UserId;
        options.RoleClaimType = Auth.KnownClaims.Role;

        options.ClaimsExtractor = (s, o, c) =>
        {
            c.Add(new(o.RoleClaimType, Auth.Roles.User)); // every identity is a valid User

            if (
                s.Identity
                    .VerifiableAddresses
                    .Any(
                        kvia =>
                            kvia.Via == "email"
                            && kvia.Value.EndsWith("@leancode.pl", false, CultureInfo.InvariantCulture)
                            && kvia.Verified
                    )
            )
            {
                c.Add(new(o.RoleClaimType, Auth.Roles.Admin));
            }
        };
    });

services.AddKratosClients(builder =>
{
    builder.AddFrontendApiClient(Config.Kratos.PublicEndpoint(config));
});

services.AddOptions<MassTransitHostOptions>().Configure(opts => opts.WaitUntilStarted = true);

services.AddMassTransit(cfg =>
{
    cfg.ConfigureLeanPipeFunnelConsumers();

    if (hostEnv.IsDevelopment())
    {
        if (Config.MassTransit.RabbitMq.Url(config) is Uri url)
        {
            cfg.UsingRabbitMq(
                (ctx, cfg) =>
                {
                    cfg.Host(url);
                    ConfigureBusCommon(ctx, cfg);
                }
            );
        }
    }
    else
    {
        cfg.UsingAzureServiceBus(
            (ctx, cfg) =>
            {
                cfg.Host(
                    new Uri(Config.MassTransit.AzureServiceBus.Endpoint(config)),
                    host =>
                    {
                        host.RetryLimit = 5;
                        host.RetryMinBackoff = TimeSpan.FromSeconds(3);
                        host.TokenCredential = DefaultLeanCodeCredential.Create(config);
                    }
                );

                ConfigureBusCommon(ctx, cfg);
            }
        );
    }

    static void ConfigureBusCommon<TEndpoint>(IBusRegistrationContext ctx, IBusFactoryConfigurator<TEndpoint> cfg)
        where TEndpoint : IReceiveEndpointConfigurator
    {
        cfg.ConfigureEndpoints(ctx);
    }
});

services.AddAzureClients(cfg =>
{
    if (!hostEnv.IsDevelopment())
    {
        cfg.UseCredential(DefaultLeanCodeCredential.Create(config));
    }
});

services.AddHealthChecks();

var otlp = Config.Telemetry.OtlpEndpoint(config);

if (!string.IsNullOrWhiteSpace(otlp))
{
    services
        .AddOpenTelemetry()
        .ConfigureResource(r => r.AddService("ExampleApp.LeanPipeFunnel", serviceInstanceId: Environment.MachineName))
        .WithTracing(builder =>
        {
            builder
                .AddProcessor<IdentityTraceAttributesFromBaggageProcessor>()
                .AddAspNetCoreInstrumentation(
                    opts => opts.Filter = ctx => !ctx.Request.Path.StartsWithSegments("/live")
                )
                .AddHttpClientInstrumentation()
                .AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName)
                .AddLeanCodeTelemetry()
                .AddOtlpExporter(cfg => cfg.Endpoint = new(otlp));
        })
        .WithMetrics(builder =>
        {
            builder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddMeter(MassTransit.Monitoring.InstrumentationOptions.MeterName)
                .AddOtlpExporter(cfg => cfg.Endpoint = new(otlp));
        });
}

var app = appBuilder.Build();

app.UseRouting().UseAuthentication();

app.MapGet("/", VersionHandler.HandleAsync);
app.MapGet("/live/ready", ReadinessProbe.HandleAsync);
app.MapHealthChecks("/live/health");

app.MapLeanPipe(
    "/leanpipe",
    opts =>
    {
        opts.Transports = HttpTransportType.WebSockets;
    }
);

app.Run();
