using ExampleApp.Api.Handlers;
using ExampleApp.Core.Contracts;
using ExampleApp.Core.Contracts.Projects;
using ExampleApp.Core.Domain.Events;
using ExampleApp.Core.Services;
using ExampleApp.Core.Services.DataAccess;
using ExampleApp.Core.Services.DataAccess.Serialization;
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
using LeanCode.Startup.MicrosoftDI;
using LeanCode.ViewRenderer.Razor;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Hosting;

namespace ExampleApp.Api;

public class Startup : LeanStartup
{
    private static readonly RazorViewRendererOptions ViewOptions = new("Templates");

    public static readonly TypesCatalog AllHandlers = new(typeof(CoreDbContext));
    public static readonly TypesCatalog Api = new(typeof(CreateProject));
    public static readonly TypesCatalog Domain = new(typeof(EmployeeAssignedToAssignment));

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
            .AddForceUpdate(
                new AndroidVersionsConfiguration(new Version(1, 0), new Version(1, 1)),
                new IOSVersionsConfiguration(new Version(1, 0), new Version(1, 1))
            );

        services.AddLeanPipe(Api, AllHandlers);

        services.AddFluentValidation(AllHandlers);
        services.AddStringLocalizer(LocalizationConfiguration.For<Strings.Strings>());
        services.AddFCM<Guid>(fcm => fcm.AddTokenStore<CoreDbContext>());
        services.AddCoreServices(Config.PostgreSQL.ConnectionString(Configuration));
        services.AddApiServices(Configuration, hostEnv);

        services.AddCQRSMassTransitIntegration(cfg =>
        {
            cfg.AddEntityFrameworkOutbox<CoreDbContext>(outboxCfg =>
            {
                outboxCfg.LockStatementProvider =
                    new LeanCode.CQRS.MassTransitRelay.LockProviders.CustomPostgresLockStatementProvider();
                outboxCfg.UseBusOutbox();
            });

            cfg.AddConsumersWithDefaultConfiguration(
                AllHandlers.Assemblies.ToArray(),
                typeof(DefaultConsumerDefinition<>)
            );

            if (hostEnv.IsDevelopment())
            {
                cfg.AddDelayedMessageScheduler();
                cfg.UsingInMemory(
                    (ctx, cfg) =>
                    {
                        cfg.UseDelayedMessageScheduler();
                        ConfigureBusCommon<IInMemoryBusFactoryConfigurator, IInMemoryReceiveEndpointConfigurator>(
                            ctx,
                            cfg
                        );
                    }
                );
            }
            else
            {
                cfg.AddServiceBusMessageScheduler();
                cfg.UsingAzureServiceBus(
                    (ctx, cfg) =>
                    {
                        cfg.Host(
                            new Uri(Config.MassTransit.AzureServiceBus.Endpoint(Configuration)),
                            host =>
                            {
                                host.RetryLimit = 5;
                                host.RetryMinBackoff = TimeSpan.FromSeconds(3);
                                host.TokenCredential = DefaultLeanCodeCredential.Create(Configuration);
                            }
                        );

                        cfg.UseServiceBusMessageScheduler();
                        ConfigureBusCommon<IServiceBusBusFactoryConfigurator, IServiceBusReceiveEndpointConfigurator>(
                            ctx,
                            cfg
                        );
                    }
                );
            }

            static void ConfigureBusCommon<TConfigurator, TReceiveConfigurator>(
                IBusRegistrationContext ctx,
                TConfigurator cfg
            )
                where TConfigurator : IBusFactoryConfigurator<TReceiveConfigurator>
                where TReceiveConfigurator : IReceiveEndpointConfigurator
            {
                cfg.ConfigureEndpoints(ctx);
                cfg.ConfigureJsonSerializerOptions(KnownConverters.AddAll);
                cfg.ConnectBusObservers(ctx);
            }
        });
    }

    protected override void ConfigureApp(IApplicationBuilder app)
    {
        app.UseRouting().UseForwardedHeaders().UseCors(ApiModule.ApiCorsPolicy);

        if (Directory.Exists("/.well-known"))
        {
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = new SymlinkResolvingPhysicalFileProvider(
                        new("/.well-known", ExclusionFilters.DotPrefixed)
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

                endpoints.MapRemoteCqrs(
                    "/api",
                    cqrs =>
                    {
                        cqrs.Commands = c =>
                            c.CQRSTrace()
                                .Secure()
                                .Validate()
                                .CommitTransaction<CoreDbContext>()
                                .Audit<CoreDbContext>()
                                .PublishEvents();
                        cqrs.Queries = c => c.CQRSTrace().Secure();
                        cqrs.Operations = c =>
                            c.CQRSTrace().Secure().CommitTransaction<CoreDbContext>().PublishEvents();
                    }
                );

                endpoints.MapLeanPipe("/leanpipe");
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
        endpointConfigurator.UseMessageRetry(
            r => r.Immediate(1).Incremental(3, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5))
        );
        endpointConfigurator.UseEntityFrameworkOutbox<CoreDbContext>(context);
        endpointConfigurator.UseDomainEventsPublishing(context);
        endpointConfigurator.UseAuditLogs<CoreDbContext>(context);
    }
}
