using System.Data.Common;
using ExampleApp.Examples.Services.DataAccess;
using LeanCode.AzureIdentity;
using LeanCode.EFMigrator;
using LeanCode.Npgsql.ActiveDirectory;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ExampleApp.Migrations;

public class Program
{
    public static void Main(string[] args)
    {
        MigrationsConfig.ConnectionStringKey = "PostgreSQL:ConnectionString";

        new ExamplesMigrator().Run(args);
    }
}

internal class ExamplesMigrator : Migrator
{
    private static readonly NpgsqlDataSource DataSource = CreateDataSource();

    private static NpgsqlDataSource CreateDataSource()
    {
        var connectionString =
            MigrationsConfig.GetConnectionString()
            ?? throw new InvalidOperationException("Failed to find connection string.");

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

        if (dataSourceBuilder.ConnectionStringBuilder.Password is null)
        {
            dataSourceBuilder.UseAzureActiveDirectoryAuthentication(DefaultLeanCodeCredential.CreateFromEnvironment());
        }

        return dataSourceBuilder.Build();
    }

    protected override DbConnection GetDbConnection() => DataSource.CreateConnection();

    protected override void MigrateAll() => Migrate<ExamplesDbContext, ExamplesDbContextFactory>();

    private class ExamplesDbContextFactory : BaseFactory<ExamplesDbContext, ExamplesDbContextFactory>
    {
        public override DbContextOptionsBuilder<ExamplesDbContext> UseDbProvider(
            DbContextOptionsBuilder<ExamplesDbContext> builder
        )
        {
            return builder.UseNpgsql(DataSource, cfg => cfg.MigrationsAssembly(AssemblyName).SetPostgresVersion(15, 0));
        }
    }
}
