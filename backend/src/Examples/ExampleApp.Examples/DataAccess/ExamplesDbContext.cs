using System.Text.Json;
using ExampleApp.Examples.DataAccess.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.DataAccess;

public partial class ExamplesDbContext : DbContext
{
    public DbSet<KratosIdentity> KratosIdentities => Set<KratosIdentity>();

    public ExamplesDbContext(DbContextOptions<ExamplesDbContext> options)
        : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        //-:cnd:noEmit
#if CHECK_EFCORE_PG_2977
#error Check if this workaround is still required.
#else
        // workaround for https://github.com/npgsql/efcore.pg/issues/2977
        configurationBuilder.Properties<JsonElement?>().HaveColumnType("jsonb");
#endif
        //+:cnd:noEmit

#if Example
        ConfigureExampleAppConventions(configurationBuilder);
#endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("citext");

        modelBuilder.AddTransactionalOutboxEntities();

        modelBuilder.Entity<KratosIdentity>(e =>
        {
            e.OwnsMany(
                ki => ki.RecoveryAddresses,
                b =>
                {
                    b.WithOwner().HasForeignKey(a => a.IdentityId);
                    b.HasKey(a => a.Id);
                    b.ToTable("KratosIdentityRecoveryAddresses");
                }
            );

            e.OwnsMany(
                ki => ki.VerifiableAddresses,
                b =>
                {
                    b.WithOwner().HasForeignKey(a => a.IdentityId);
                    b.HasKey(a => a.Id);
                    b.ToTable("KratosIdentityVerifiableAddresses");
                }
            );

            e.Property<uint>("xmin").IsRowVersion();
        });

#if Example
        OnExampleModelCreating(modelBuilder);
#endif
    }
}
