using System.Diagnostics;
using System.Reflection;
using LeanCode.Components;
using LeanCode.Components.Startup;
using LeanCode.CQRS.RemoteHttp.Client;
using LeanCode.IntegrationTestHelpers;
using ExampleApp.Core.Services.DataAccess;
using ExampleApp.IntegrationTests.Overrides;
using ExampleApp.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Events;
using LeanCode;
using System.Text.Json;
using MassTransit.Testing.Implementations;
using Xunit;

namespace ExampleApp.IntegrationTests
{
    public class ExampleAppTestApp : LeanCodeTestFactory<Startup>
    {
        protected virtual JsonSerializerOptions JsonOptions { get; } = new();

        protected override ConfigurationOverrides Configuration { get; } =
            new(
                LogEventLevel.Debug,
                true,
                connectionStringBase: "PostgreSQL__ConnectionStringBase",
                connectionStringKey: "PostgresSQL:ConnectionString"
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

        public const string UserEmail = "test@leancode.pl";
        public const string UserPassword = "long_test_password123!";

        protected override IEnumerable<Assembly> GetTestAssemblies()
        {
            yield return typeof(ExampleAppTestApp).Assembly;
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            return LeanProgram
                .BuildMinimalHost<TestStartup>()
                .ConfigureDefaultLogging(
                    projectName: "ExampleApp-tests",
                    destructurers: new TypesCatalog(typeof(Program))
                )
                .UseEnvironment(Environments.Development);
        }

        public override HttpClient CreateApiClient()
        {
            var client = CreateDefaultClient(new Uri(UrlHelper.Concat("http://localhost/", ApiBaseAddress)));

            if (!string.IsNullOrEmpty(CurrentUserToken))
            {
                client.DefaultRequestHeaders.Authorization = new("Bearer", CurrentUserToken);
            }

            return client;
        }

        public override HttpQueriesExecutor CreateQueriesExecutor()
        {
            return new(CreateApiClient(), JsonOptions);
        }

        public override HttpCommandsExecutor CreateCommandsExecutor()
        {
            return new(CreateApiClient(), JsonOptions);
        }

        public virtual HttpOperationsExecutor CreateOperationsExecutor()
        {
            return new(CreateApiClient(), JsonOptions);
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            using (var scope = Services.CreateScope())
            {
                await CreateTestUserAsync(scope.ServiceProvider);
            }
        }

        public Task AuthenticateAsync()
        {
            throw new NotImplementedException("Needs better Kratos integration.");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                services.AddTransient<DbContext>(sp => sp.GetService<CoreDbContext>());
            });
        }

        private Task CreateTestUserAsync(IServiceProvider services)
        {
            throw new NotImplementedException("Needs better Kratos integration.");
        }

        public async Task WaitForProcessingAsync()
        {
            using var scope = Services.CreateScope();
            var monitor = scope.ServiceProvider.GetRequiredService<IBusActivityMonitor>();
            // Allow some processing time, selected arbitrarily
            var res = await monitor.AwaitBusInactivity(TimeSpan.FromSeconds(30));
            Assert.True(res, "The service bus should finish processing in allowed time.");
        }
    }

    public class AuthenticatedExampleAppTestApp : ExampleAppTestApp
    {
        public HttpQueriesExecutor Query { get; private set; } = default!;
        public HttpCommandsExecutor Command { get; private set; } = default!;
        public HttpOperationsExecutor Operation { get; private set; } = default!;

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await AuthenticateAsync();

            Query = CreateQueriesExecutor();
            Command = CreateCommandsExecutor();
            Operation = CreateOperationsExecutor();

            await WaitForProcessingAsync();
        }

        public override async ValueTask DisposeAsync()
        {
            Command = default!;
            Query = default!;
            Operation = default!;
            await base.DisposeAsync();
        }
    }

    public class UnauthenticatedExampleAppTestApp : ExampleAppTestApp
    {
        public HttpQueriesExecutor Query { get; private set; } = default!;
        public HttpCommandsExecutor Command { get; private set; } = default!;
        public HttpOperationsExecutor Operation { get; private set; } = default!;

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            Query = CreateQueriesExecutor();
            Command = CreateCommandsExecutor();
            Operation = CreateOperationsExecutor();

            await WaitForProcessingAsync();
        }

        public override async ValueTask DisposeAsync()
        {
            Command = default!;
            Query = default!;
            Operation = default!;
            await base.DisposeAsync();
        }
    }
}
