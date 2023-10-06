using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using LeanCode.CQRS.MassTransitRelay;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;
using static LeanCode.AuditLogs.Tests.AuditLogsFilterTests;

namespace LeanCode.AuditLogs.Tests;

public sealed class AuditLogsMiddlewareTests : IAsyncLifetime, IDisposable
{
    private const string SomeId = "some_id";
    private const string ActorId = "actor_id";
    private const string TestPath = "/test";
    private static readonly JsonSerializerOptions Options =
        new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = false,
        };

    private readonly IHost host;
    private readonly ITestHarness harness;
    private readonly TestServer server;
    private static readonly TestEntity TestEntity = new() { Id = SomeId };

    public AuditLogsMiddlewareTests()
    {
        // TODO: Set time correctly
        // TestTimeProvider.ActivateFake(new DateTimeOffset(2023, 10, 6, 12, 13, 12, 0, TimeSpan.Zero));
        host = new HostBuilder()
            .ConfigureWebHost(webHost =>
            {
                webHost
                    .UseTestServer()
                    .ConfigureServices(cfg =>
                    {
                        cfg.AddDbContext<TestDbContext>();
                        cfg.AddTransient<IAuditLogStorage, StubAuditLogStorage>();
                        cfg.AddMassTransitTestHarness(ConfigureMassTransit);
                        cfg.AddRouting();
                    })
                    .Configure(app =>
                    {
                        app.Audit<TestDbContext>();
                        app.UseRouting()
                            .UseEndpoints(
                                e =>
                                    e.MapPost(
                                        TestPath,
                                        (ctx) =>
                                        {
                                            var dbContext = ctx.RequestServices.GetService<TestDbContext>();
                                            dbContext.Add(TestEntity);
                                            return Task.CompletedTask;
                                        }
                                    )
                            );
                        app.Run(ctx =>
                        {
                            return Task.CompletedTask;
                        });
                    });
            })
            .Build();

        server = host.GetTestServer();

        harness = host.Services.GetRequiredService<ITestHarness>();
    }

    private static void ConfigureMassTransit(IBusRegistrationConfigurator cfg)
    {
        cfg.AddConsumersWithDefaultConfiguration(
            new[] { typeof(AuditLogsConsumer).Assembly },
            typeof(DefaultConsumerDefinition<>)
        );

        cfg.UsingInMemory(
            (ctx, busCfg) =>
            {
                busCfg.ConfigureEndpoints(ctx, new DefaultEndpointNameFormatter("InMemory"));
                busCfg.ConnectBusObservers(ctx);
            }
        );
    }

    [Fact]
    public async Task Ensure_that_audit_log_is_collected_correctly()
    {
        await harness.Start();
        await server.SendAsync(ctx =>
        {
            ctx.Request.Method = "POST";
            ctx.Request.Path = TestPath;
        });

        harness.Published
            .Select<AuditLogMessage>()
            .Should()
            .ContainSingle()
            .Which.Context.Message.Should()
            .BeEquivalentTo(
                new
                {
                    EntityChanged = new
                    {
                        Ids = new string[] { SomeId },
                        Type = typeof(TestEntity).FullName,
                        EntityState = "Added",
                        Changes = JsonSerializer.SerializeToDocument(TestEntity, Options),
                    },
                    ActionName = TestPath,
                    ActorId = null as string,
                    TraceId = null as string,
                    SpanId = null as string,
                    // TODO: Set time correctly
                    // DateOccurred = Time.NowWithOffset,
                },
                opt => opt.ComparingByMembers<JsonElement>()
            );
    }

    public Task InitializeAsync() => host.StartAsync();

    public Task DisposeAsync() => host.StopAsync();

    public void Dispose()
    {
        server.Dispose();
        host.Dispose();
    }
}
