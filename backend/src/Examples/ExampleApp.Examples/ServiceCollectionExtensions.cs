using System.Globalization;
using Azure.Core;
using ExampleApp.Examples.Configuration;
using ExampleApp.Examples.DataAccess;
using ExampleApp.Examples.DataAccess.Queries;
using ExampleApp.Examples.DataAccess.Repositories;
using ExampleApp.Examples.DataAccess.Serialization;
using ExampleApp.Examples.Handlers.Identities;
using LeanCode.AuditLogs;
using LeanCode.AzureIdentity;
using LeanCode.DomainModels.DataAccess;
using LeanCode.DomainModels.Model;
using LeanCode.Kratos.Client.Api;
using LeanCode.Kratos.Client.Client;
using LeanCode.Kratos.Client.Extensions;
using LeanCode.Kratos.Client.Model;
using LeanCode.Npgsql.ActiveDirectory;
using LeanCode.OpenTelemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using static ExampleApp.Examples.Contracts.Auth;
#if Example
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using ServiceProvider = ExampleApp.Examples.Domain.Booking.ServiceProvider;
#endif

namespace ExampleApp.Examples;

public static class ServiceCollectionExtensions
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
                        s.Identity?.VerifiableAddresses?.Any(kvia =>
                            kvia.Via == KratosVerifiableIdentityAddress.ViaEnum.Email
                            && kvia.Value.EndsWith("@leancode.pl", false, CultureInfo.InvariantCulture)
                        ) ?? false
                    )
                    {
                        c.Add(new(o.RoleClaimType, Roles.Admin));
                    }
#endif
                };
            });

        services.AddKratos(builder =>
        {
            builder.UseProvider<NullTokenProvider, ApiKeyToken>();
            builder.AddKratosHttpClients(builder: hcb =>
                _ = hcb.Name switch
                {
                    nameof(ICourierApi) or nameof(IIdentityApi) => hcb.ConfigureHttpClient(hc =>
                        hc.BaseAddress = new(AppConfig.Kratos.AdminEndpoint(config))
                    ),
                    nameof(IFrontendApi) or nameof(IMetadataApi) => hcb.ConfigureHttpClient(hc =>
                        hc.BaseAddress = new(AppConfig.Kratos.PublicEndpoint(config))
                    ),
                    _ => throw new NotSupportedException($"Unexpected client name: '{hcb.Name}'."),
                }
            );
        });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        var otlp = AppConfig.Telemetry.OtlpEndpoint(config);

        if (!string.IsNullOrWhiteSpace(otlp))
        {
            services
                .AddOpenTelemetry()
                .ConfigureResource(r =>
                    r.AddService("ExampleApp.Examples", serviceInstanceId: Environment.MachineName)
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
            cfg.AddBlobServiceClient(AppConfig.BlobStorage.ConnectionString(config));
            cfg.AddTableServiceClient(AppConfig.BlobStorage.ConnectionString(config));

            if (!hostEnv.IsDevelopment())
            {
                cfg.UseCredential(DefaultLeanCodeCredential.Create(config));
            }
        });

        services.AddAzureStorageAuditLogs(
            new AzureBlobAuditLogStorageConfiguration(
                AppConfig.AuditLogs.ContainerName(config),
                AppConfig.AuditLogs.TableName(config)
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
                cfg.WithOrigins(AppConfig.CORS.AllowedOrigins(config))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromMinutes(60));
            }
        );
    }

    public static void AddExamplesServices(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(sp =>
        {
            var builder = new NpgsqlDataSourceBuilder(connectionString);

            if (builder.ConnectionStringBuilder.Password is null)
            {
                builder.UseAzureActiveDirectoryAuthentication(sp.GetRequiredService<TokenCredential>());
            }

            builder.EnableDynamicJson().ConfigureJsonOptions(KnownConverters.AddAll(new()));

            return builder.Build();
        });

        services.AddDbContext<ExamplesDbContext>(
            (sp, opts) =>
                opts
                //-:cnd:noEmit
#if DEBUG
                .EnableSensitiveDataLogging()
#endif
                    //+:cnd:noEmit
                    .UseNpgsql(
                        sp.GetRequiredService<NpgsqlDataSource>(),
                        cfg =>
                            cfg.MigrationsAssembly(typeof(ExamplesDbContext).Assembly.FullName)
                                .SetPostgresVersion(15, 0)
                    )
        );

#if Example
        services.AddRepository<ProjectId, Project, ProjectsRepository>();
        services.AddRepository<EmployeeId, Employee, EmployeesRepository>();
        services.AddRepository<ServiceProviderId, ServiceProvider, ServiceProvidersRepository>();
        services.AddRepository<CalendarDayId, CalendarDay, CalendarDaysRepository>();
        services.AliasScoped<ICalendarDayByDate, CalendarDaysRepository>();
#endif
    }

    private static void AddRepository<TId, TEntity, TImplementation>(this IServiceCollection services)
        where TId : struct
        where TEntity : class, IAggregateRootWithoutOptimisticConcurrency<TId>
        where TImplementation : class, IRepository<TEntity, TId>
    {
        services.AddScoped<TImplementation>();
        services.AddScoped<IRepository<TEntity, TId>>(sp => sp.GetRequiredService<TImplementation>());
    }

    private static void AliasScoped<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : TService
    {
        services.AddScoped<TService>(sp => sp.GetRequiredService<TImplementation>());
    }
}
