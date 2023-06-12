using Autofac;
using ExampleApp.Core.Services.DataAccess;
using LeanCode.Components;
using LeanCode.DomainModels.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ExampleApp.Core.Services;

public class CoreModule : AppModule
{
    private readonly string connectionString;

    public CoreModule(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var builder = new NpgsqlDataSourceBuilder(connectionString);

            if (sp.GetService<Azure.Core.TokenCredential>() is { } credential)
            {
                builder.UseAzureActiveDirectoryAuthentication(credential);
            }

            return builder.Build();
        });

        services.AddDbContext<CoreDbContext>(
            opts =>
                opts
#if DEBUG
                .EnableSensitiveDataLogging()
#endif
                    .UseNpgsql(cfg => cfg.MigrationsAssembly("ExampleApp.Migrations").SetPostgresVersion(14, 0))
        );
    }

    protected override void Load(ContainerBuilder builder)
    {
        var self = typeof(CoreModule).Assembly;

        builder.Register(c => c.Resolve<CoreDbContext>()).AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(self).AsClosedTypesOf(typeof(IRepository<,>)).AsImplementedInterfaces().AsSelf();
    }
}
