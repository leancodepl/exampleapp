using Azure.Core;
using ExampleApp.Examples.Services.DataAccess;
using ExampleApp.Examples.Services.DataAccess.Serialization;
using LeanCode.DomainModels.DataAccess;
using LeanCode.DomainModels.Model;
using LeanCode.Npgsql.ActiveDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
#if Example
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using ExampleApp.Examples.Services.DataAccess.Repositories;
using ServiceProvider = ExampleApp.Examples.Domain.Booking.ServiceProvider;
#endif

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
                        cfg =>
                            cfg.MigrationsAssembly(typeof(ExamplesDbContext).Assembly.FullName)
                                .SetPostgresVersion(15, 0)
                    )
        );

#if Example
        services.AddRepository<ProjectId, Project, ProjectsRepository>();
        services.AddRepository<EmployeeId, Employee, EmployeesRepository>();
        services.AddRepository<ServiceProviderId, ServiceProvider, ServiceProvidersRepository>();
        services.AddRepository<CalendarDayId, CalendarDay, CalendarDaysRepository>();
#endif
    }

    private static void AddRepository<TId, TEntity, TImplementation>(this IServiceCollection services)
        where TId : struct
        where TEntity : class, IAggregateRootWithoutOptimisticConcurrency<TId>
        where TImplementation : class, IRepository<TEntity, TId>
    {
        services.AddScoped<TImplementation>();
        services.AddScoped<IRepository<TEntity, TId>>(sp => sp.GetRequiredService<TImplementation>());
    }
}
