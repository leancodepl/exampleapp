using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using ExampleApp.Examples.Contracts;
using ExampleApp.Examples.DataAccess;
using ExampleApp.Examples.IntegrationTests.Helpers;
using LeanCode.AuditLogs;
using LeanCode.CQRS.MassTransitRelay;
using LeanCode.CQRS.RemoteHttp.Client;
using LeanCode.IntegrationTestHelpers;
using LeanCode.Logging;
using LeanCode.Pipe.TestClient;
using LeanCode.Startup.MicrosoftDI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog.Events;

namespace ExampleApp.Examples.IntegrationTests;

public class ExampleAppTestApp : LeanCodeTestFactory<Startup>
{
    public readonly Guid SuperAdminId = Guid.Parse("4d3b45e6-a2c1-4d6a-9e23-94e0d9f8ca01");

    public bool SkipDbContextInitialization { get; init; } = false;

    protected override TestConnectionString ConnectionStringConfig { get; } =
        new(
            connectionStringBaseKey: "PostgreSQL:ConnectionStringBase",
            connectionStringKey: "PostgreSQL:ConnectionString"
        );

    protected override ConfigurationOverrides ConfigurationOverrides { get; } =
        ConfigurationOverrides.LoggingOverrides();

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
            .ConfigureDefaultLogging("ExampleApp.Examples.IntegrationTests", [typeof(Program).Assembly])
            .UseEnvironment(Environments.Development)
            .ConfigureAppConfiguration(
                (context, builder) =>
                {
                    builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                }
            );
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseSolutionRelativeContentRoot("tests/Examples/ExampleApp.Examples.IntegrationTests");

        builder.ConfigureServices(services =>
        {
            if (!SkipDbContextInitialization)
            {
                // This needs to be `Insert`ed because we expect the initializer to be run  anything else,
                // especially before MassTransit outbox process, and this method is executed after
                // the `Startup.ConfigureServices`.
                services.Insert(
                    0,
                    ServiceDescriptor.Singleton<IHostedService, DbContextInitializer<ExamplesDbContext>>()
                );
            }

            services.AddBusActivityMonitor();

            services.AddAuthentication(TestAuthenticationHandler.SchemeName).AddTestAuthenticationHandler();
            services.Replace(ServiceDescriptor.Singleton<IAuditLogStorage>(new AuditLogStorageMock()));
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

    public override async ValueTask InitializeAsync()
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

public class MultiUserExampleAppTestApp : ExampleAppTestApp
{
    private readonly IReadOnlyList<ClaimsPrincipal> principals;

    public IReadOnlyList<HttpQueriesExecutor> Queries { get; private set; } = [];
    public IReadOnlyList<HttpCommandsExecutor> Commands { get; private set; } = [];
    public IReadOnlyList<HttpOperationsExecutor> Operations { get; private set; } = [];

    public MultiUserExampleAppTestApp()
        : this(2) { }

    public MultiUserExampleAppTestApp(int principalsCount)
    {
        principals = Enumerable.Range(0, principalsCount).Select(_ => TestPrincipal()).ToList();
    }

    public override async ValueTask InitializeAsync()
    {
        TestPrincipal();

        await base.InitializeAsync();
        Queries = principals.Select(p => CreateQueriesExecutor(hc => hc.UseTestAuthorization(p))).ToList();
        Commands = principals.Select(p => CreateCommandsExecutor(hc => hc.UseTestAuthorization(p))).ToList();
        Operations = principals.Select(p => CreateOperationsExecutor(hc => hc.UseTestAuthorization(p))).ToList();

        await WaitForBusAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        Queries = [];
        Commands = [];
        Operations = [];
        await base.DisposeAsync();
    }

    private static ClaimsPrincipal TestPrincipal()
    {
        return new(
            new ClaimsIdentity(
                [
                    new(Auth.KnownClaims.UserId, Guid.NewGuid().ToString()),
                    new(Auth.KnownClaims.Role, Auth.Roles.User),
                    new(Auth.KnownClaims.Role, Auth.Roles.Admin),
                ],
                TestAuthenticationHandler.SchemeName,
                Auth.KnownClaims.UserId,
                Auth.KnownClaims.Role
            )
        );
    }
}

public class UnauthenticatedExampleAppTestApp : ExampleAppTestApp
{
    public HttpQueriesExecutor Query { get; private set; } = default!;
    public HttpCommandsExecutor Command { get; private set; } = default!;
    public HttpOperationsExecutor Operation { get; private set; } = default!;
    public LeanPipeTestClient LeanPipe { get; private set; } = default!;

    public override async ValueTask InitializeAsync()
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
