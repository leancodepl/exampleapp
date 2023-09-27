using ExampleApp.Core.Domain.Employees;
using ExampleApp.Core.Domain.Projects;
using ExampleApp.Core.Services.DataAccess.Entities;
using LeanCode.DomainModels.EF;
using LeanCode.DomainModels.Ids;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace ExampleApp.Core.Services.DataAccess;

public class CoreDbContext : DbContext
{
    public DbSet<KratosIdentity> KratosIdentities => Set<KratosIdentity>();

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Project> Projects => Set<Project>();

    public CoreDbContext(DbContextOptions<CoreDbContext> options)
        : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ConfigureId<EmployeeId>(configurationBuilder);
        ConfigureId<ProjectId>(configurationBuilder);
        ConfigureId<AssignmentId>(configurationBuilder);

        static PropertiesConfigurationBuilder<TId> ConfigureId<TId>(ModelConfigurationBuilder configurationBuilder)
            where TId : struct, IPrefixedTypedId<TId>
        {
            return configurationBuilder
                .Properties<TId>()
                .HaveColumnType(JsonNamingPolicy.SnakeCaseLower.ConvertName(typeof(TId).Name))
                .HaveConversion<PrefixedTypedIdConverter<TId>, PrefixedTypedIdComparer<TId>>();
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasPostgresExtension("citext");

        builder.AddInboxStateEntity();
        builder.AddOutboxStateEntity();
        builder.AddOutboxMessageEntity();

        builder.Entity<KratosIdentity>(e =>
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

        builder.Entity<Employee>(e =>
        {
            e.HasKey(t => t.Id);

            e.IsOptimisticConcurrent(addRowVersion: false);
            e.Property<uint>("xmin").IsRowVersion();
        });

        builder.Entity<Project>(e =>
        {
            e.HasKey(t => t.Id);

            e.OwnsMany(
                p => p.Assignments,
                inner =>
                {
                    inner.WithOwner(a => a.ParentProject).HasForeignKey(a => a.ParentProjectId);

                    inner.ToTable("Assignments");
                }
            );

            e.IsOptimisticConcurrent(addRowVersion: false);
            e.Property<uint>("xmin").IsRowVersion();
        });
    }
}
