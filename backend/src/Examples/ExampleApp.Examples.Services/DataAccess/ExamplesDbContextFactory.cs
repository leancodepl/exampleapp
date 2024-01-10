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
    private static readonly NpgsqlDataSource DataSource = CreateDataSource();

    private static NpgsqlDataSource CreateDataSource()
    {
        var connectionString =
            Environment.GetEnvironmentVariable(ConnectionStringKey)
            ?? throw new InvalidOperationException("Failed to find connection string.");

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        if (dataSourceBuilder.ConnectionStringBuilder.Password is null)
        {
            dataSourceBuilder.UseAzureActiveDirectoryAuthentication(DefaultLeanCodeCredential.CreateFromEnvironment());
        }

        return dataSourceBuilder.Build();
    }

    public ExamplesDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ExamplesDbContext>();

        optionsBuilder.UseNpgsql(DataSource, cfg => cfg.SetPostgresVersion(15, 0));

        return new ExamplesDbContext(optionsBuilder.Options);
    }
}
