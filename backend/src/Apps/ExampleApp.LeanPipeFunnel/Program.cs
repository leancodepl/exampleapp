using System.Globalization;
using ExampleApp.LeanPipeFunnel;
using LeanCode.AzureIdentity;
using LeanCode.Logging;
using LeanCode.Pipe;
using LeanCode.Pipe.Funnel.Instance;
using MassTransit;
using MassTransit.SignalR;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Azure;

var appBuilder = WebApplication.CreateBuilder(args);
var hostBuilder = appBuilder.Host;

hostBuilder
    .AddAppConfigurationFromAzureKeyVaultOnNonDevelopmentEnvironment()
    .ConfigureDefaultLogging("ExampleApp.LeanPipeFunnel", new[] { typeof(Program).Assembly });

var hostEnv = appBuilder.Environment;
var services = appBuilder.Services;
var config = appBuilder.Configuration;

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
                s.Identity.VerifiableAddresses.Any(
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
    builder.AddIdentityApiClient(Config.Kratos.AdminEndpoint(config));
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

services.AddMappedConfiguration(config);

if (!hostEnv.IsDevelopment())
{
    services.AddSingleton(DefaultLeanCodeCredential.Create(config));
}

var app = appBuilder.Build();

app.UseRouting().UseAuthentication();

app.MapLeanPipe(
    "/leanpipe",
    opts =>
    {
        opts.Transports = HttpTransportType.WebSockets;
    }
);

app.Run();
