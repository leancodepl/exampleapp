using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Polly;
using Serilog;

namespace ExampleApp.Examples.IntegrationTests;

public class DbContextInitializer<T>(IServiceProvider serviceProvider) : IHostedService
    where T : DbContext
{
    private static readonly IAsyncPolicy CreatePolicy = Policy
        .Handle((NpgsqlException e) => e.IsTransient)
        .WaitAndRetryAsync([TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(3.0)]);

    private readonly ILogger logger = Log.ForContext<DbContextInitializer<T>>();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<T>();
        logger.Information("Creating database for context {ContextType}", context.GetType());
        await CreatePolicy.ExecuteAsync(
            async token =>
            {
                await context.Database.EnsureDeletedAsync(token);
                await context.Database.EnsureCreatedAsync(token);

                if (context.Database.GetDbConnection() is NpgsqlConnection connection)
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        await connection.OpenAsync(token);
                        await connection.ReloadTypesAsync();
                        await connection.CloseAsync();
                    }
                    else
                    {
                        await connection.ReloadTypesAsync();
                    }
                }
            },
            cancellationToken
        );
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<T>();
        logger.Information("Dropping database for context {ContextType}", context.GetType());
        // We skip the cancellation token to properly delete the database even if test fails
        await context.Database.EnsureDeletedAsync(CancellationToken.None);
    }
}
