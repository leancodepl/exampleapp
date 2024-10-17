using ExampleApp.Examples.Configuration;
using ExampleApp.Examples.Contracts;
using ExampleApp.Examples.DataAccess;
using ExampleApp.Examples.DataAccess.Serialization;
using ExampleApp.Examples.Handlers.HealthCheck;
using ExampleApp.Examples.Handlers.Identities;
using LeanCode.AppRating;
using LeanCode.AuditLogs;
using LeanCode.AzureIdentity;
using LeanCode.Components;
using LeanCode.CQRS.AspNetCore;
using LeanCode.CQRS.MassTransitRelay;
using LeanCode.CQRS.MassTransitRelay.Middleware;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.Firebase.FCM;
using LeanCode.ForceUpdate;
using LeanCode.Localization;
using LeanCode.OpenTelemetry;
using LeanCode.Pipe;
using LeanCode.Pipe.Funnel.FunnelledService;
using LeanCode.SendGrid;
using LeanCode.Startup.MicrosoftDI;
using LeanCode.ViewRenderer.Razor;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Hosting;
using SendGrid;

namespace ExampleApp.Examples;

public class Startup : LeanStartup
{
    public static readonly TypesCatalog AllHandlers = new(typeof(ExamplesDbContext));
    public static readonly TypesCatalog Api = new(typeof(PaginatedQuery<>));

    private readonly IWebHostEnvironment hostEnv;

    public Startup(IWebHostEnvironment hostEnv, IConfiguration config)
        : base(config)
    {
        this.hostEnv = hostEnv;
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services
            .AddCQRS(Api, AllHandlers)
#if Example
            .AddAppRating<Guid, ExamplesDbContext, UserIdExtractor>()
#endif
            .AddForceUpdate(
                new AndroidVersionsConfiguration(new Version(1, 0), new Version(1, 1)),
                new IOSVersionsConfiguration(new Version(1, 0), new Version(1, 1))
            );

        var leanPipeFunnelEnabled = AppConfig.LeanPipe.EnableLeanPipeFunnel(Configuration);

        if (leanPipeFunnelEnabled)
        {
            services.AddFunnelledLeanPipe(Api, AllHandlers);
        }
        else
        {
            services.AddLeanPipe(Api, AllHandlers);
        }

        services.AddSendGridClient(
            AppConfig.SendGrid.ApiKey(Configuration) is var sendGridApiKey && !string.IsNullOrEmpty(sendGridApiKey)
                ? new SendGridClientOptions { ApiKey = sendGridApiKey, HttpErrorAsException = true }
                : new SendGridClientOptions()
        );

        services.AddRazorViewRenderer(new("Templates/Email"));

        services.AddFluentValidation(AllHandlers);
        services.AddStringLocalizer(LocalizationConfiguration.For<Strings.Strings>());
        services.AddExamplesServices(AppConfig.PostgreSQL.ConnectionString(Configuration));
#if Example
        services.AddFCM<Guid>(fcm => fcm.AddTokenStore<ExamplesDbContext>());
#endif
        services.AddApiServices(Configuration, hostEnv);

        services.AddCQRSMassTransitIntegration(cfg =>
        {
            cfg.AddEntityFrameworkOutbox<ExamplesDbContext>(outboxCfg =>
            {
                outboxCfg.LockStatementProvider =
                    new LeanCode.CQRS.MassTransitRelay.LockProviders.CustomPostgresLockStatementProvider();
                outboxCfg.UseBusOutbox();
            });

            cfg.AddAuditLogsConsumer();
#if Example
            cfg.AddAppRatingConsumers<Guid>();
#endif

            if (leanPipeFunnelEnabled)
            {
                cfg.AddFunnelledLeanPipeConsumers("ExampleApp.Examples.Api", Api.Assemblies);
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

    protected override void ConfigureApp(IApplicationBuilder app)
    {
        app.UseRouting().UseForwardedHeaders().UseCors(ServiceCollectionExtensions.ApiCorsPolicy);

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
                            c.CQRSTrace()
                                .Secure()
                                .Validate()
                                .CommitTransaction<ExamplesDbContext>()
                                .PublishEvents()
                                .Audit<ExamplesDbContext>();
                        cqrs.Queries = c => c.CQRSTrace().Secure();
                        cqrs.Operations = c =>
                            c.CQRSTrace()
                                .Secure()
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
}

public class DefaultConsumerDefinition<TConsumer> : ConsumerDefinition<TConsumer>
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

#if Example
public sealed class UserIdExtractor : IUserIdExtractor<Guid>
{
    public Guid Extract(HttpContext httpContext) => httpContext.GetUserId();
}
#endif
