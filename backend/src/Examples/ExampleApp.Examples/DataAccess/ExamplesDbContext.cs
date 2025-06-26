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
#if Example
        ConfigureExampleAppConventions(configurationBuilder);
#endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("citext");
        modelBuilder.HasPostgresExtension("pg_trgm");

        modelBuilder.AddTransactionalOutboxEntities();

        modelBuilder.Entity<KratosIdentity>(e =>
        {
            e.HasIndex(ki => ki.CreatedAt);

            if (false) // EF Core can't deal with indexes on inner properties :(
            {
#pragma warning disable CS0162, IDE0035
                e.HasIndex(ki => ki.Traits.GetProperty("email").GetString())
                    .HasMethod("gin")
                    .HasOperators("gin_trgm_ops");
                e.HasIndex(ki => ki.Traits.GetProperty("given_name").GetString())
                    .HasMethod("gin")
                    .HasOperators("gin_trgm_ops");
                e.HasIndex(ki => ki.Traits.GetProperty("family_name").GetString())
                    .HasMethod("gin")
                    .HasOperators("gin_trgm_ops");
#pragma warning restore CS0162, IDE0035
            }

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
