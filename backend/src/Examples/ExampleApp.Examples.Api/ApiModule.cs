using System.Globalization;
using ExampleApp.Examples.Api.Handlers;
using ExampleApp.Examples.Services.DataAccess;
using LeanCode.AuditLogs;
using LeanCode.AzureIdentity;
using LeanCode.OpenTelemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using static ExampleApp.Examples.Contracts.Auth;

namespace ExampleApp.Examples.Api;

internal static class ApiModule
{
    internal const string ApiCorsPolicy = "Api";

    public static void AddApiServices(
        this IServiceCollection services,
        IConfiguration config,
        IWebHostEnvironment hostEnv
    )
    {
        services.AddCors(cors => ConfigureCORS(cors, config));
        services.AddRouting();
        services.AddHealthChecks().AddDbContextCheck<ExamplesDbContext>();

        services
            .AddAuthentication()
            .AddKratos(options =>
            {
                options.NameClaimType = KnownClaims.UserId;
                options.RoleClaimType = KnownClaims.Role;

                options.ClaimsExtractor = (s, o, c) =>
                {
                    c.Add(new(o.RoleClaimType, Roles.User)); // every identity is a valid User
#if Example
                    if (
                        s.Identity.VerifiableAddresses.Any(kvia =>
                            kvia.Via == "email"
                            && kvia.Value.EndsWith("@leancode.pl", false, CultureInfo.InvariantCulture)
                            && kvia.Verified
                        )
                    )
                    {
                        c.Add(new(o.RoleClaimType, Roles.Admin));
                    }
#endif
                };
            });

        services.AddKratosClients(builder =>
        {
            builder.AddFrontendApiClient(Config.Kratos.PublicEndpoint(config));
            builder.AddIdentityApiClient(Config.Kratos.AdminEndpoint(config));
        });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        var otlp = Config.Telemetry.OtlpEndpoint(config);

        if (!string.IsNullOrWhiteSpace(otlp))
        {
            services
                .AddOpenTelemetry()
                .ConfigureResource(r =>
                    r.AddService("ExampleApp.Examples.Api", serviceInstanceId: Environment.MachineName)
                )
                .WithTracing(builder =>
                {
                    builder
                        .AddProcessor<IdentityTraceAttributesFromBaggageProcessor>()
                        .AddAspNetCoreInstrumentation(opts =>
                            opts.Filter = ctx => !ctx.Request.Path.StartsWithSegments("/live")
                        )
                        .AddHttpClientInstrumentation()
                        .AddNpgsql()
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

        services.AddAzureClients(cfg =>
        {
            cfg.AddBlobServiceClient(Config.BlobStorage.ConnectionString(config));
            cfg.AddTableServiceClient(Config.BlobStorage.ConnectionString(config));

            if (!hostEnv.IsDevelopment())
            {
                cfg.UseCredential(DefaultLeanCodeCredential.Create(config));
            }
        });

        services.AddAzureStorageAuditLogs(
            new AzureBlobAuditLogStorageConfiguration(
                Config.AuditLogs.ContainerName(config),
                Config.AuditLogs.TableName(config)
            )
        );

        services.AddConfigCat(true);

        services.AddSingleton<LeanCode.CQRS.Security.IRoleRegistration, AppRoles>();
        services.AddScoped<KratosIdentitySyncHandler>();
        services.AddMappedConfiguration(config, hostEnv);

        if (!hostEnv.IsDevelopment())
        {
            services.AddSingleton(DefaultLeanCodeCredential.Create(config));
        }
    }

    private static void ConfigureCORS(CorsOptions opts, IConfiguration config)
    {
        opts.AddPolicy(
            ApiCorsPolicy,
            cfg =>
            {
                cfg.WithOrigins(Config.CORS.AllowedOrigins(config))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromMinutes(60));
            }
        );
    }
}
