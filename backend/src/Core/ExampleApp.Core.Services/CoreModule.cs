using Autofac;
using ExampleApp.Core.Services.DataAccess;
using LeanCode.Components;
using LeanCode.DomainModels.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddDbContext<CoreDbContext>(
            opts => opts.UseSqlServer(connectionString, cfg => cfg.MigrationsAssembly("ExampleApp.Migrations"))
        );
    }

    protected override void Load(ContainerBuilder builder)
    {
        var self = typeof(CoreModule).Assembly;

        builder.Register(c => c.Resolve<CoreDbContext>()).AsImplementedInterfaces();

        builder
            .RegisterAssemblyTypes(self)
            .AsClosedTypesOf(typeof(IRepository<,>))
            .AsImplementedInterfaces()
            .AsSelf();
    }
}
