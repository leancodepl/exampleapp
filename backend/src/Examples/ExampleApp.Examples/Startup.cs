using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Claims;
using ExampleApp.Examples.Configuration;
using ExampleApp.Examples.Contracts;
using ExampleApp.Examples.DataAccess;
using ExampleApp.Examples.DataAccess.Blobs;
using ExampleApp.Examples.DataAccess.Serialization;
using ExampleApp.Examples.Handlers.HealthCheck;
using ExampleApp.Examples.Handlers.Identities;
using ExampleApp.Examples.Notifications;
using ExampleApp.Examples.Observability;
using LeanCode.AuditLogs;
using LeanCode.AzureIdentity;
using LeanCode.Components;
using LeanCode.CQRS.AspNetCore;
using LeanCode.CQRS.MassTransitRelay;
using LeanCode.CQRS.MassTransitRelay.Middleware;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using LeanCode.DomainModels.EF;
using LeanCode.DomainModels.Model;
using LeanCode.ForceUpdate;
using LeanCode.Kratos.Client.Api;
using LeanCode.Kratos.Client.Client;
using LeanCode.Kratos.Client.Extensions;
using LeanCode.Kratos.Client.Model;
using LeanCode.Localization;
using LeanCode.Logging;
using LeanCode.Npgsql.ActiveDirectory;
using LeanCode.OpenTelemetry;
using LeanCode.Pipe;
using LeanCode.Pipe.Funnel.FunnelledService;
using LeanCode.SendGrid;
using LeanCode.Startup.MicrosoftDI;
using LeanCode.ViewRenderer.Razor;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Hosting;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using SendGrid;
#if Example
using ExampleApp.Examples.Contracts.Booking.Reservations.Authorization;
using ExampleApp.Examples.DataAccess.Queries;
using ExampleApp.Examples.DataAccess.Repositories;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using ExampleApp.Examples.Handlers.Booking.Reservations.Authorization;
using LeanCode.AppRating;
using LeanCode.Firebase.FCM;
using LeanCode.NotificationCenter;
using LeanCode.NotificationCenter.DataAccess;
using LeanCode.UserIdExtractors;
using Booking = ExampleApp.Examples.Domain.Booking;
#endif

namespace ExampleApp.Examples;

public class Startup(IWebHostEnvironment hostEnv, IConfiguration config) : LeanStartup(config)
{
    private const string ApiCorsPolicy = "Api";

    private static readonly TypesCatalog AllHandlers = new(typeof(ExamplesDbContext));
    public static readonly TypesCatalog Api = new(typeof(PaginatedQuery<>));

    public override void ConfigureServices(IServiceCollection services)
    {
        services
            .AddCQRS(Api, AllHandlers)
#if Example
            .AddAppRating<Guid, ExamplesDbContext, UserIdExtractor>()
            .AddNotificationCenter<Guid>()
#endif
            .AddForceUpdate(
                new AndroidVersionsConfiguration(new Version(1, 0), new Version(1, 1)),
                new IOSVersionsConfiguration(new Version(1, 0), new Version(1, 1))
            );

#if Example
        services
            .AddNotificationCenter<Guid>(new(Consts.FromEmail, Consts.FromName, null))
            .AddUserConfigurationProvider<NotificationsUserConfigurationProvider>()
            .Configure<SampleNotificationPayload>(true, true, true);
        services.AddSingleton<LeanCode.UserIdExtractors.IUserIdExtractor<Guid>, UserIdExtractor>();
#endif

        services.AddSendGridClient(
            AppConfig.SendGrid.ApiKey(Configuration) is var sendGridApiKey && !string.IsNullOrEmpty(sendGridApiKey)
                ? new SendGridClientOptions { ApiKey = sendGridApiKey, HttpErrorAsException = true }
                : new SendGridClientOptions()
        );
        services.AddRazorViewRenderer(new("Templates/Email"));
        services.AddFluentValidation(AllHandlers);
        services.AddStringLocalizer(LocalizationConfiguration.For<Strings.Strings>());
        services.AddRouting();
        services.AddHealthChecks().AddDbContextCheck<ExamplesDbContext>();
        services.AddAzureStorageAuditLogs(
            new AzureBlobAuditLogStorageConfiguration(
                AppConfig.AuditLogs.ContainerName(Configuration),
                AppConfig.AuditLogs.TableName(Configuration)
            )
        );
        services.AddConfigCat(true);
        services.AddMemoryCache();

#if Example
        services.AddFCM<Guid>(fcm => fcm.AddTokenStore<ExamplesDbContext>());
#endif
        AddAuthorizers(services);
        AddAzureClients(services);
        AddCors(services);
        AddDbContext(services);
        AddKratos(services);
        AddLeanPipe(services);
        AddMassTransit(services);
        AddOpenTelemetry(services);
        AddRepositories(services);
        AddBlobStorage(services);

        services.AddSingleton<LeanCode.CQRS.Security.IRoleRegistration, AppRoles>();
        services.AddScoped<KratosIdentitySyncHandler>();

        services.AddMappedConfiguration(Configuration, hostEnv);

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });
    }

    [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue")]
    protected override void ConfigureApp(IApplicationBuilder app)
    {
        app.UseRouting().UseForwardedHeaders().UseCors(ApiCorsPolicy);

        if (Directory.Exists("/.well-known"))
        {
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = new SymlinkResolvingPhysicalFileProvider(
                        "/.well-known",
                        ExclusionFilters.DotPrefixed
                    ),
                    RequestPath = "/.well-known",
                    DefaultContentType = "application/json",
                    ServeUnknownFileTypes = true,
                    RedirectToAppendTrailingSlash = false,
                }
            );
        }

        app.UseAuthentication()
            .UseUserIdLogsCorrelation(Auth.KnownClaims.UserId)
            .UseIdentityTraceAttributes(Auth.KnownClaims.UserId, Auth.KnownClaims.Role)
            .UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", VersionHandler.HandleAsync);
                endpoints.MapGet("/live/ready", ReadinessProbe.HandleAsync);
                endpoints.MapHealthChecks("/live/health");

                endpoints.MapPost(
                    "/kratos/sync-identity",
                    ctx => ctx.RequestServices.GetRequiredService<KratosIdentitySyncHandler>().HandleAsync(ctx)
                );

                endpoints.MapRemoteCQRS(
                    "/api",
                    cqrs =>
                    {
                        cqrs.Commands = c =>
                            c.Secure()
                                .Validate()
                                .CommitTransaction<ExamplesDbContext>()
                                .PublishEvents()
                                .Audit<ExamplesDbContext>();
                        cqrs.Queries = c => c.Secure();
                        cqrs.Operations = c =>
                            c.Secure()
                                .CommitTransaction<ExamplesDbContext>()
                                .PublishEvents()
                                .Audit<ExamplesDbContext>();
                    }
                );

                if (!AppConfig.LeanPipe.EnableLeanPipeFunnel(Configuration))
                {
                    endpoints.MapLeanPipe("/leanpipe");
                }
            });
    }

    private void AddAuthorizers(IServiceCollection services)
    {
#if Example
        services.AddScoped<
            AuthorizeWhenOwnsReservationAttribute.IWhenOwnsReservation,
            AuthorizeWhenOwnsReservationAuthorizer
        >();
#endif
    }

    private void AddAzureClients(IServiceCollection services)
    {
        services.AddAzureClients(cfg =>
        {
            cfg.AddBlobServiceClient(AppConfig.BlobStorage.ConnectionString(Configuration));
            cfg.AddTableServiceClient(AppConfig.BlobStorage.ConnectionString(Configuration));

            if (!hostEnv.IsDevelopment())
            {
                cfg.UseCredential(DefaultLeanCodeCredential.Create(Configuration));
            }
        });
    }

    private void AddCors(IServiceCollection services)
    {
        services.AddCors(cors =>
            cors.AddPolicy(
                ApiCorsPolicy,
                cfg =>
                {
                    cfg.WithOrigins(AppConfig.CORS.AllowedOrigins(Configuration))
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromMinutes(60));
                }
            )
        );
    }

    private void AddDbContext(IServiceCollection services)
    {
        services.AddSingleton(_ =>
        {
            var builder = new NpgsqlDataSourceBuilder(AppConfig.PostgreSQL.ConnectionString(Configuration));

            // Default to localhost in case no connection string is provided to make the migrations bundle build
            builder.ConnectionStringBuilder.Host ??= "localhost";

            if (
                builder.ConnectionStringBuilder.Host.EndsWith(".azure.com")
                && builder.ConnectionStringBuilder.Password is null
            )
            {
                builder.UseAzureActiveDirectoryAuthentication(DefaultLeanCodeCredential.Create(Configuration));
            }

            builder.EnableDynamicJson().ConfigureJsonOptions(KnownConverters.AddAll(new()));

            return builder.Build();
        });

        services.AddDbContext<ExamplesDbContext>(
            (sp, opts) =>
                opts.AddTimestampTzExpressionInterceptor()
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

        services.AddScoped<INotificationsDbContext<Guid>>(sp => sp.GetRequiredService<ExamplesDbContext>());
    }

    private void AddKratos(IServiceCollection services)
    {
        services
            .AddAuthentication()
            .AddKratos(options =>
            {
                options.NameClaimType = Auth.KnownClaims.UserId;
                options.RoleClaimType = Auth.KnownClaims.Role;

                options.ClaimsExtractor = (s, o, c) =>
                {
                    c.Add(new(o.RoleClaimType, Auth.Roles.User)); // every identity is a valid User
#if Example
                    if (
                        s.Identity?.VerifiableAddresses?.Any(kvia =>
                            kvia.Via == KratosVerifiableIdentityAddress.ViaEnum.Email
                            && kvia.Value.EndsWith("@leancode.pl", false, CultureInfo.InvariantCulture)
                        ) ?? false
                    )
                    {
                        c.Add(new(o.RoleClaimType, Auth.Roles.Admin));
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
                        hc.BaseAddress = new(AppConfig.Kratos.AdminEndpoint(Configuration))
                    ),
                    nameof(IFrontendApi) or nameof(IMetadataApi) => hcb.ConfigureHttpClient(hc =>
                        hc.BaseAddress = new(AppConfig.Kratos.PublicEndpoint(Configuration))
                    ),
                    _ => throw new NotSupportedException($"Unexpected client name: '{hcb.Name}'."),
                }
            );
        });
    }

    private void AddLeanPipe(IServiceCollection services)
    {
        var leanPipeFunnelEnabled = AppConfig.LeanPipe.EnableLeanPipeFunnel(Configuration);

        if (leanPipeFunnelEnabled)
        {
            services.AddFunnelledLeanPipe(Api, AllHandlers);
        }
        else
        {
            services.AddLeanPipe(Api, AllHandlers);
        }
    }

    private void AddMassTransit(IServiceCollection services)
    {
        services.AddCQRSMassTransitIntegration(cfg =>
        {
            cfg.AddEntityFrameworkOutbox<ExamplesDbContext>(outboxCfg =>
            {
                outboxCfg.LockStatementProvider = new PostgresLockStatementProvider();
                outboxCfg.UseBusOutbox();
            });

            cfg.AddAuditLogsConsumer();
#if Example
            cfg.AddAppRatingConsumers<Guid>();
            cfg.AddNotificationCenter<Guid>();
#endif

            var leanPipeFunnelEnabled = AppConfig.LeanPipe.EnableLeanPipeFunnel(Configuration);
            if (leanPipeFunnelEnabled)
            {
                cfg.AddFunnelledLeanPipeConsumers("ExampleApp.Examples", Api.Assemblies);
            }

            cfg.AddConsumersWithDefaultConfiguration(
                AllHandlers.Assemblies.ToArray(),
                typeof(DefaultConsumerDefinition<>)
            );

            if (hostEnv.IsDevelopment())
            {
                cfg.AddDelayedMessageScheduler();

                if (AppConfig.MassTransit.RabbitMq.Url(Configuration) is Uri url)
                {
                    cfg.UsingRabbitMq(
                        (ctx, cfg) =>
                        {
                            cfg.Host(url);
                            cfg.UseDelayedMessageScheduler();
                            ConfigureBusCommon(ctx, cfg);
                        }
                    );
                }
                else
                {
                    cfg.UsingInMemory(
                        (ctx, cfg) =>
                        {
                            cfg.UseDelayedMessageScheduler();
                            ConfigureBusCommon(ctx, cfg);
                        }
                    );
                }
            }
            else
            {
                cfg.AddServiceBusMessageScheduler();
                cfg.UsingAzureServiceBus(
                    (ctx, cfg) =>
                    {
                        cfg.Host(
                            new Uri(AppConfig.MassTransit.AzureServiceBus.Endpoint(Configuration)),
                            host =>
                            {
                                host.RetryLimit = 5;
                                host.RetryMinBackoff = TimeSpan.FromSeconds(3);
                                host.TokenCredential = DefaultLeanCodeCredential.Create(Configuration);
                            }
                        );

                        cfg.UseServiceBusMessageScheduler();
                        ConfigureBusCommon(ctx, cfg);
                    }
                );
            }

            static void ConfigureBusCommon<TEndpoint>(
                IBusRegistrationContext ctx,
                IBusFactoryConfigurator<TEndpoint> cfg
            )
                where TEndpoint : IReceiveEndpointConfigurator
            {
                cfg.ConfigureEndpoints(ctx);
                cfg.ConfigureJsonSerializerOptions(KnownConverters.AddAll);
                cfg.ConnectBusObservers(ctx);
            }
        });
    }

    private void AddOpenTelemetry(IServiceCollection services)
    {
        var otlp = AppConfig.Telemetry.OtlpEndpoint(Configuration);

        if (!string.IsNullOrWhiteSpace(otlp))
        {
            services
                .AddOpenTelemetry()
                .ConfigureResource(r => r.AddService("ExampleApp.Examples", serviceInstanceId: Environment.MachineName))
                .WithTracing(builder =>
                {
                    builder
                        .AddProcessor<IdentityTraceAttributesFromBaggageProcessor>()
                        .AddAspNetCoreInstrumentation(o =>
                        {
                            o.RecordException = true;
                        })
                        .AddProcessor<HealthCheckActivityFilteringProcessor>()
                        .AddHttpClientInstrumentation(o =>
                        {
                            o.RecordException = true;
                            o.FilterHttpRequestMessage = _ =>
                                Activity.Current?.Parent?.Source.Name != "Azure.Core.Http";
                        })
                        .AddSource("Azure.*") // Azure SDK
                        .AddNpgsql()
                        .AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName)
                        .AddProcessor<MassTransitActivityFilteringProcessor>()
                        .AddLeanCodeInstrumentation()
                        .SetErrorStatusOnException()
                        .AddOtlpExporter(cfg => cfg.Endpoint = new(otlp));
                })
                .WithMetrics(builder =>
                {
                    builder
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation()
                        .AddNpgsqlInstrumentation()
                        .AddMeter(MassTransit.Monitoring.InstrumentationOptions.MeterName)
                        .AddOtlpExporter(cfg => cfg.Endpoint = new(otlp));
                });
        }
    }

    private void AddRepositories(IServiceCollection services)
    {
#if Example
        services.AddRepository<ProjectId, Project, ProjectsRepository>();
        services.AddRepository<EmployeeId, Employee, EmployeesRepository>();
        services.AddRepository<ServiceProviderId, Booking.ServiceProvider, ServiceProvidersRepository>();
        services.AddRepository<CalendarDayId, CalendarDay, CalendarDaysRepository>();
        services.AddRepository<ReservationId, Reservation, ReservationsRepository>();
        services.AliasScoped<ICalendarDayByDate, CalendarDaysRepository>();
#endif
    }

    private void AddBlobStorage(IServiceCollection services)
    {
        services.AddScoped<BlobStorageDelegationKeyProvider>();
#if Example
        services.AddScoped<ServiceProviderLogoStorage>();
#endif
    }
}

file class DefaultConsumerDefinition<TConsumer> : ConsumerDefinition<TConsumer>
    where TConsumer : class, IConsumer
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<TConsumer> consumerConfigurator,
        IRegistrationContext context
    )
    {
        endpointConfigurator.UseMessageRetry(r =>
            r.Immediate(1).Incremental(3, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5))
        );
        endpointConfigurator.UseEntityFrameworkOutbox<ExamplesDbContext>(context);
        endpointConfigurator.UseDomainEventsPublishing(context);
        endpointConfigurator.UseAuditLogs<ExamplesDbContext>(context);
    }
}

file static class ServiceCollectionExtensions
{
    public static void AddRepository<TId, TEntity, TImplementation>(this IServiceCollection services)
        where TId : struct
        where TEntity : class, IAggregateRootWithoutOptimisticConcurrency<TId>
        where TImplementation : class, IRepository<TEntity, TId>
    {
        services.AddScoped<TImplementation>();
        services.AddScoped<IRepository<TEntity, TId>>(sp => sp.GetRequiredService<TImplementation>());
    }

    public static void AliasScoped<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : TService
    {
        services.AddScoped<TService>(sp => sp.GetRequiredService<TImplementation>());
    }
}

#if Example
public sealed class UserIdExtractor : LeanCode.AppRating.IUserIdExtractor<Guid>, LeanCode.UserIdExtractors.IUserIdExtractor<Guid>
{
    public Guid Extract(HttpContext httpContext) => httpContext.GetUserId();

    public bool TryExtract(HttpContext httpContext, out Guid userId) => httpContext.TryGetUserId(out userId);
    public Guid Extract(ClaimsPrincipal user)
    {
        var claim = user.FindFirstValue(Auth.KnownClaims.UserId);

        ArgumentException.ThrowIfNullOrEmpty(claim);

        return Guid.Parse(claim);
    }
}
#endif
