using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using ExampleApp.Api;
using ExampleApp.Core.Contracts;
using ExampleApp.Core.Services.DataAccess;
using LeanCode.CQRS.MassTransitRelay;
using LeanCode.CQRS.RemoteHttp.Client;
using LeanCode.IntegrationTestHelpers;
using LeanCode.Logging;
using LeanCode.Pipe.TestClient;
using LeanCode.Startup.MicrosoftDI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog.Events;

namespace ExampleApp.IntegrationTests;

public class ExampleAppTestApp : LeanCodeTestFactory<Startup>
{
    public readonly Guid SuperAdminId = Guid.Parse("4d3b45e6-a2c1-4d6a-9e23-94e0d9f8ca01");

    public bool SkipDbContextOverrideAndInitialization { get; init; } = false;

    protected override ConfigurationOverrides Configuration { get; } =
        new(
            connectionStringBase: "PostgreSQL__ConnectionStringBase",
            connectionStringKey: "PostgreSQL:ConnectionString",
            LogEventLevel.Debug,
            true
        );

    static ExampleAppTestApp()
    {
        if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("WAIT_FOR_DEBUGGER")))
        {
            Console.WriteLine("Waiting for debugger to be attached...");

            while (!Debugger.IsAttached)
            {
                Thread.Sleep(100);
            }
        }
    }

    protected override IEnumerable<Assembly> GetTestAssemblies()
    {
        yield return typeof(ExampleAppTestApp).Assembly;
    }

    protected override IHostBuilder CreateHostBuilder()
    {
        return LeanProgram
            .BuildMinimalHost<Startup>()
            .ConfigureDefaultLogging(projectName: "ExampleApp-tests", destructurers: new[] { typeof(Program).Assembly })
            .UseEnvironment(Environments.Development);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            if (!SkipDbContextOverrideAndInitialization)
            {
                services.RemoveAll<CoreDbContext>();
                services.AddScoped<CoreDbContext, Overrides.CoreDbContext>();
                services.AddHostedService<DbContextInitializer<CoreDbContext>>();
            }

            services.AddBusActivityMonitor();

            services.AddAuthentication(TestAuthenticationHandler.SchemeName).AddTestAuthenticationHandler();
        });
    }
}

public class AuthenticatedExampleAppTestApp : ExampleAppTestApp
{
    private ClaimsPrincipal claimsPrincipal = new();

    public HttpQueriesExecutor Query { get; private set; } = default!;
    public HttpCommandsExecutor Command { get; private set; } = default!;
    public HttpOperationsExecutor Operation { get; private set; } = default!;
    public LeanPipeTestClient LeanPipe { get; private set; } = default!;

    public AuthenticatedExampleAppTestApp() { }

    public override async Task InitializeAsync()
    {
        AuthenticateAsTestSuperUser();

        void ConfigureClient(HttpClient hc) => hc.UseTestAuthorization(claimsPrincipal);

        await base.InitializeAsync();

        Query = CreateQueriesExecutor(ConfigureClient);
        Command = CreateCommandsExecutor(ConfigureClient);
        Operation = CreateOperationsExecutor(ConfigureClient);
        LeanPipe = new(
            new("http://localhost/leanpipe"),
            Startup.Api,
            hco =>
            {
                hco.HttpMessageHandlerFactory = _ => Server.CreateHandler();
                // TODO: Add a helper for that in the CoreLibrary and use it here and below
                hco.Headers.Add(
                    "Authorization",
                    new AuthenticationHeaderValue(
                        TestAuthenticationHandler.SchemeName,
                        TestAuthenticationHandler.SerializePrincipal(claimsPrincipal)
                    ).ToString()
                );
            }
        );

        await WaitForBusAsync();
    }

    public void AuthenticateAsTestSuperUser()
    {
        claimsPrincipal = new(
            new ClaimsIdentity(
                new Claim[]
                {
                    new(Auth.KnownClaims.UserId, SuperAdminId.ToString()),
                    new(Auth.KnownClaims.Role, Auth.Roles.User),
                    new(Auth.KnownClaims.Role, Auth.Roles.Admin),
                },
                TestAuthenticationHandler.SchemeName,
                Auth.KnownClaims.UserId,
                Auth.KnownClaims.Role
            )
        );
    }

    public override async ValueTask DisposeAsync()
    {
        Command = default!;
        Query = default!;
        Operation = default!;
        await LeanPipe.DisposeAsync();
        await base.DisposeAsync();
    }
}

public class UnauthenticatedExampleAppTestApp : ExampleAppTestApp
{
    public HttpQueriesExecutor Query { get; private set; } = default!;
    public HttpCommandsExecutor Command { get; private set; } = default!;
    public HttpOperationsExecutor Operation { get; private set; } = default!;
    public LeanPipeTestClient LeanPipe { get; private set; } = default!;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        Query = CreateQueriesExecutor();
        Command = CreateCommandsExecutor();
        Operation = CreateOperationsExecutor();
        LeanPipe = new(
            new("http://localhost/leanpipe"),
            Startup.Api,
            hco =>
            {
                hco.HttpMessageHandlerFactory = _ => Server.CreateHandler();
            }
        );

        await WaitForBusAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        Command = default!;
        Query = default!;
        Operation = default!;
        await LeanPipe.DisposeAsync();
        await base.DisposeAsync();
    }
}
