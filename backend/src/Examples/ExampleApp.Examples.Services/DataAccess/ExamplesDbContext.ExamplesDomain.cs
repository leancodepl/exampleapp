using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using LeanCode.AppRating.DataAccess;
using LeanCode.DomainModels.EF;
using LeanCode.DomainModels.Ids;
using LeanCode.Firebase.FCM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleApp.Examples.Services.DataAccess;

public partial class ExamplesDbContext : IAppRatingStore<Guid>
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Project> Projects => Set<Project>();

    public DbSet<ServiceProvider> ServiceProviders => Set<ServiceProvider>();
    public DbSet<CalendarDay> CalendarDays => Set<CalendarDay>();
    public DbSet<Timeslot> Timeslots => Set<Timeslot>();

    public DbSet<PushNotificationTokenEntity<Guid>> PushNotificationTokens => Set<PushNotificationTokenEntity<Guid>>();
    public DbSet<AppRating<Guid>> AppRatings => Set<AppRating<Guid>>();

    private void ConfigureExampleAppConventions(ModelConfigurationBuilder configurationBuilder)
    {
        ConfigureId<EmployeeId>(configurationBuilder);
        ConfigureId<ProjectId>(configurationBuilder);
        ConfigureId<AssignmentId>(configurationBuilder);
        ConfigureId<ServiceProviderId>(configurationBuilder);
        ConfigureId<CalendarDayId>(configurationBuilder);
        ConfigureId<TimeslotId>(configurationBuilder);
    }

    private void OnExampleModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigurePushNotificationTokenEntity<Guid>(false);
        modelBuilder.ConfigureAppRatingEntity<Guid>(SqlDbType.PostgreSql);

        modelBuilder.Entity<Employee>(e =>
        {
            e.HasKey(t => t.Id);

            e.IsOptimisticConcurrent(addRowVersion: false);
            e.Property<uint>("xmin").IsRowVersion();
        });

        modelBuilder.Entity<Project>(e =>
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

        modelBuilder.Entity<ServiceProvider>(e =>
        {
            e.HasKey(t => t.Id);

            e.ComplexProperty(t => t.Location);

            e.IsOptimisticConcurrent(addRowVersion: false);
            e.Property<uint>("xmin").IsRowVersion();
        });

        modelBuilder.Entity<CalendarDay>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasAlternateKey(t => new { t.ServiceProviderId, t.Date });

            e.HasMany(t => t.Timeslots).WithOne(t => t.CalendarDay);

            e.IsOptimisticConcurrent(addRowVersion: false);
            e.Property<uint>("xmin").IsRowVersion();
        });

        modelBuilder.Entity<Timeslot>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasOne(t => t.CalendarDay).WithMany(t => t.Timeslots);

            e.ComplexProperty(t => t.Price);

            e.HasIndex(t => new
            {
                t.ServiceProviderId,
                t.CalendarDayId,
                t.Date,
                t.StartTime,
            });
        });
    }

    private static PropertiesConfigurationBuilder<TId> ConfigureId<TId>(ModelConfigurationBuilder configurationBuilder)
        where TId : struct, IPrefixedTypedId<TId>
    {
        return configurationBuilder
            .Properties<TId>()
            .HaveColumnType("citext")
            .HaveConversion<PrefixedTypedIdConverter<TId>, PrefixedTypedIdComparer<TId>>();
    }
}
