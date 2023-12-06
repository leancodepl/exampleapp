using System.Text.Json;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using ExampleApp.Examples.Services.DataAccess.Entities;
using LeanCode.AppRating.DataAccess;
using LeanCode.DomainModels.EF;
using LeanCode.DomainModels.Ids;
using LeanCode.Firebase.FCM;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleApp.Examples.Services.DataAccess;

public class ExamplesDbContext : DbContext, IAppRatingStore<Guid>
{
    public DbSet<PushNotificationTokenEntity<Guid>> PushNotificationTokens => Set<PushNotificationTokenEntity<Guid>>();

    public DbSet<KratosIdentity> KratosIdentities => Set<KratosIdentity>();

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Project> Projects => Set<Project>();

    public DbSet<AppRating<Guid>> AppRatings => Set<AppRating<Guid>>();

    public ExamplesDbContext(DbContextOptions<ExamplesDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // optionsBuilder.AddTimestampTzExpressionInterceptor();
    }

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

        ConfigureId<EmployeeId>(configurationBuilder);
        ConfigureId<ProjectId>(configurationBuilder);
        ConfigureId<AssignmentId>(configurationBuilder);

        static PropertiesConfigurationBuilder<TId> ConfigureId<TId>(ModelConfigurationBuilder configurationBuilder)
            where TId : struct, IPrefixedTypedId<TId>
        {
            return configurationBuilder
                .Properties<TId>()
                .HaveColumnType("citext")
                .HaveConversion<PrefixedTypedIdConverter<TId>, PrefixedTypedIdComparer<TId>>();
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasPostgresExtension("citext");

        builder.AddTransactionalOutboxEntities();

        builder.ConfigurePushNotificationTokenEntity<Guid>(false);

        builder.ConfigureAppRatingEntity<Guid>(SqlDbType.PostgreSql);

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
