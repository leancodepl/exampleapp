using Autofac;
using LeanCode.AzureIdentity;
using LeanCode.Components;
using LeanCode.OpenTelemetry;
using ExampleApp.Api.Handlers;
using ExampleApp.Core.Services.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Npgsql;
using static ExampleApp.Core.Contracts.Auth;

namespace ExampleApp.Api;

internal class ApiModule : AppModule
{
    internal const string ApiCorsPolicy = "Api";

    private readonly IConfiguration config;
    private readonly IWebHostEnvironment hostEnv;

    public ApiModule(IConfiguration config, IWebHostEnvironment hostEnv)
    {
        this.config = config;
        this.hostEnv = hostEnv;
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(ConfigureCORS);
        services.AddRouting();
        services.AddHealthChecks().AddDbContextCheck<CoreDbContext>();

        services
            .AddAuthentication()
            .AddKratos(options =>
            {
                options.NameClaimType = KnownClaims.UserId;
                options.RoleClaimType = KnownClaims.Role;

                options.ClaimsExtractor = (s, o, c) =>
                {
                    c.Add(new(o.RoleClaimType, Roles.User)); // every identity is a valid User
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
                .ConfigureResource(r => r.AddService("ExampleApp.Api", serviceInstanceId: Environment.MachineName))
                .WithTracing(builder =>
                {
                    builder
                        .AddAspNetCoreInstrumentation(
                            opts => opts.Filter = ctx => !ctx.Request.Path.StartsWithSegments("/live")
                        )
                        .AddHttpClientInstrumentation()
                        .AddNpgsql()
                        .AddSource("MassTransit")
                        .AddLeanCodeTelemetry()
                        .AddOtlpExporter(cfg => cfg.Endpoint = new(otlp));
                })
                .WithMetrics(builder =>
                {
                    builder
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter(cfg => cfg.Endpoint = new(otlp));
                });
        }

        services.AddAzureClients(cfg =>
        {
            cfg.AddBlobServiceClient(Config.BlobStorage.ConnectionString(config));

            if (!hostEnv.IsDevelopment())
            {
                cfg.UseCredential(DefaultLeanCodeCredential.Create(config));
            }
        });
    }

    protected override void Load(ContainerBuilder builder)
    {
        Config.RegisterMappedConfiguration(builder, config, hostEnv);

        builder.RegisterType<AppRoles>().AsImplementedInterfaces();
        builder.RegisterType<KratosIdentitySyncHandler>().AsSelf();
    }

    private void ConfigureCORS(CorsOptions opts)
    {
        opts.AddPolicy(
            ApiCorsPolicy,
            cfg =>
            {
                cfg.WithOrigins(Config.Services.AllowedOrigins(config))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromMinutes(60));
            }
        );
    }
}
