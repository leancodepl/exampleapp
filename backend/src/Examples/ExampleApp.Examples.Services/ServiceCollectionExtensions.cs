using Azure.Core;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using ExampleApp.Examples.Services.DataAccess;
using ExampleApp.Examples.Services.DataAccess.Repositories;
using ExampleApp.Examples.Services.DataAccess.Serialization;
using LeanCode.DomainModels.DataAccess;
using LeanCode.Npgsql.ActiveDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ExampleApp.Examples.Services;

public static class ServiceCollectionExtensions
{
    public static void AddExamplesServices(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(sp =>
        {
            var builder = new NpgsqlDataSourceBuilder(connectionString);

            if (builder.ConnectionStringBuilder.Password is null)
            {
                builder.UseAzureActiveDirectoryAuthentication(sp.GetRequiredService<TokenCredential>());
            }

            builder.EnableDynamicJson().ConfigureJsonOptions(KnownConverters.AddAll(new()));

            return builder.Build();
        });

        services.AddDbContext<ExamplesDbContext>(
            (sp, opts) =>
                opts
//-:cnd:noEmit
#if DEBUG
                .EnableSensitiveDataLogging()
#endif
//+:cnd:noEmit
                    .UseNpgsql(
                        sp.GetRequiredService<NpgsqlDataSource>(),
                        cfg => cfg.MigrationsAssembly("ExampleApp.Migrations").SetPostgresVersion(15, 0)
                    )
        );

        services.AddScoped<ProjectsRepository>();
        services.AddScoped<IRepository<Project, ProjectId>, ProjectsRepository>();

        services.AddScoped<EmployeesRepository>();
        services.AddScoped<IRepository<Employee, EmployeeId>, EmployeesRepository>();
    }
}
