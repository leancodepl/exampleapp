using LeanCode.AzureIdentity;
using LeanCode.Npgsql.ActiveDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace ExampleApp.Examples.Services.DataAccess;

/// <remarks> Used for `dotnet ef migrations bundle`. </remarks>
public class ExamplesDbContextFactory : IDesignTimeDbContextFactory<ExamplesDbContext>
{
    private const string ConnectionStringKey = "PostgreSQL__ConnectionString";
    private readonly NpgsqlDataSource dataSource;

    public ExamplesDbContextFactory(NpgsqlDataSource dataSource)
    {
        this.dataSource = dataSource;
    }

    public ExamplesDbContextFactory()
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(Environment.GetEnvironmentVariable(ConnectionStringKey));
        var connectionStringBuilder = dataSourceBuilder.ConnectionStringBuilder;

        connectionStringBuilder.Host ??= Environment.GetEnvironmentVariable("PGHOST") ?? "localhost";

        if (connectionStringBuilder.Host.EndsWith(".azure.com") && connectionStringBuilder.Password is null)
        {
            dataSourceBuilder.UseAzureActiveDirectoryAuthentication(DefaultLeanCodeCredential.CreateFromEnvironment());
        }

        dataSource = dataSourceBuilder.Build();
    }

    public ExamplesDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ExamplesDbContext>();

        optionsBuilder.UseNpgsql(dataSource, cfg => cfg.SetPostgresVersion(15, 0));

        return new ExamplesDbContext(optionsBuilder.Options);
    }
}
