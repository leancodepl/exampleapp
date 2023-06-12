using LeanCode.EFMigrator;
using ExampleApp.Core.Services.DataAccess;

namespace ExampleApp.Migrations;

public class Program
{
    public static void Main(string[] args)
    {
        new Migrator().Run(args);
    }
}

internal class Migrator : LeanCode.EFMigrator.Migrator
{
    protected override void MigrateAll()
    {
        Migrate<CoreDbContext, CoreDbContextFactory>();
    }
}

internal class CoreDbContextFactory : BaseFactory<CoreDbContext, CoreDbContextFactory> { }
