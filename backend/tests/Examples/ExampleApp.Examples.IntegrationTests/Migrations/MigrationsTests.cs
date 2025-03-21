using ExampleApp.Examples.DataAccess;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExampleApp.Examples.IntegrationTests.Migrations;

public class MigrationTests
{
    [Fact]
    public async Task Migrations_can_be_successfully_applied()
    {
        await using var app = new ExampleAppTestApp { SkipDbContextInitialization = true };

        await app.InitializeAsync();
        await using var scope = app.Services.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<ExamplesDbContext>();

        try
        {
            await dbContext.Database.MigrateAsync(cancellationToken: TestContext.Current.CancellationToken);
        }
        finally
        {
            await dbContext.Database.EnsureDeletedAsync(TestContext.Current.CancellationToken);
        }
    }

    [Fact]
    public async Task All_migrations_have_been_generated()
    {
        await using var app = new ExampleAppTestApp { SkipDbContextInitialization = true };

        await app.InitializeAsync();
        await using var scope = app.Services.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<ExamplesDbContext>();

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
