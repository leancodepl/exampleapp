using ExampleApp.Core.Services.DataAccess;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExampleApp.IntegrationTests.Migrations;

public class MigrationTests
{
    [Fact]
    public async Task Migrations_can_be_successfully_applied()
    {
        await using var app = new ExampleAppTestApp { SkipDbContextInitialization = true };

        await app.InitializeAsync();
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();

        try
        {
            await dbContext.Database.MigrateAsync();
        }
        finally
        {
            await dbContext.Database.EnsureDeletedAsync();
        }
    }

    [Fact]
    public async Task All_migrations_have_been_generated()
    {
        var app = new ExampleAppTestApp { SkipDbContextInitialization = true };

        await app.InitializeAsync();

        var dbContext = app.Services.GetRequiredService<CoreDbContext>();

        var migrationsAssembly = dbContext.GetService<IMigrationsAssembly>();
        var initializer = dbContext.GetService<IModelRuntimeInitializer>();
        var differ = dbContext.GetService<IMigrationsModelDiffer>();
        var designTime = dbContext.GetService<IDesignTimeModel>();

        var modelSnapshot = migrationsAssembly.ModelSnapshot ?? throw new Exception("No snapshot.");
        var model = modelSnapshot.Model ?? throw new Exception("No model.");

        if (model is IMutableModel mutableModel)
        {
            model = mutableModel.FinalizeModel();
        }

        model = initializer.Initialize(model);

        differ.GetDifferences(model.GetRelationalModel(), designTime.Model.GetRelationalModel()).Should().BeEmpty();
    }
}
