using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using LeanCode.AppRating.DataAccess;
using LeanCode.DomainModels.EF;
using LeanCode.DomainModels.Ids;
using LeanCode.Firebase.FCM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CustomerId = ExampleApp.Examples.Domain.CustomerId;

namespace ExampleApp.Examples.DataAccess;

public partial class ExamplesDbContext : IAppRatingStore<Guid>
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Project> Projects => Set<Project>();

    public DbSet<ServiceProvider> ServiceProviders => Set<ServiceProvider>();
    public DbSet<CalendarDay> CalendarDays => Set<CalendarDay>();
    public DbSet<Timeslot> Timeslots => Set<Timeslot>();

    public DbSet<Reservation> Reservations => Set<Reservation>();

    public DbSet<PushNotificationTokenEntity<Guid>> PushNotificationTokens => Set<PushNotificationTokenEntity<Guid>>();
    public DbSet<AppRating<Guid>> AppRatings => Set<AppRating<Guid>>();

    private void ConfigureExampleAppConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.ConfigureId<EmployeeId>();
        configurationBuilder.ConfigureId<ProjectId>();
        configurationBuilder.ConfigureId<AssignmentId>();
        configurationBuilder.ConfigureId<ServiceProviderId>();
        configurationBuilder.ConfigureId<CalendarDayId>();
        configurationBuilder.ConfigureId<TimeslotId>();
        configurationBuilder.ConfigureId<ReservationId>();
        configurationBuilder.ConfigureGuidId<CustomerId>();
    }

    private void OnExampleModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigurePushNotificationTokenEntity<Guid>(false);
        modelBuilder.ConfigureAppRatingEntity<Guid>(SqlDbType.PostgreSql);

        modelBuilder.Entity<Employee>(e =>
        {
            e.HasKey(t => t.Id);

            e.IsOptimisticConcurrent(addRowVersion: false);
            e.HasRowVersion();
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
            e.HasRowVersion();
        });

        modelBuilder.Entity<ServiceProvider>(e =>
        {
            e.HasKey(t => t.Id);

            e.ComplexProperty(t => t.Location);

            e.IsOptimisticConcurrent(addRowVersion: false);
            e.HasRowVersion();
        });

        modelBuilder.Entity<CalendarDay>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasAlternateKey(t => new { t.ServiceProviderId, t.Date });

            e.HasMany(t => t.Timeslots).WithOne(t => t.CalendarDay);
            e.Navigation(t => t.Timeslots).AutoInclude();

            e.IsOptimisticConcurrent(addRowVersion: false);
            e.HasRowVersion();
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

        modelBuilder.Entity<Reservation>(e =>
        {
            e.HasKey(t => t.Id);

            e.HasIndex(t => new { t.CustomerId, t.TimeslotId });
            e.HasIndex(t => new { t.CustomerId, t.Status });

            e.IsOptimisticConcurrent(addRowVersion: false);
            e.HasRowVersion();
        });
    }
}

file static class ModelConfigurationBuilderExtensions
{
    public static PropertiesConfigurationBuilder<TId> ConfigureId<TId>(
        this ModelConfigurationBuilder configurationBuilder
    )
        where TId : struct, IPrefixedTypedId<TId>
    {
        return configurationBuilder
            .Properties<TId>()
            .HaveColumnType("citext")
            .HaveConversion<PrefixedTypedIdConverter<TId>, PrefixedTypedIdComparer<TId>>();
    }

    public static PropertiesConfigurationBuilder<TId> ConfigureGuidId<TId>(
        this ModelConfigurationBuilder configurationBuilder
    )
        where TId : struct, IRawTypedId<Guid, TId>
    {
        return configurationBuilder
            .Properties<TId>()
            .HaveColumnType("uuid")
            .HaveConversion<RawTypedIdConverter<Guid, TId>, RawTypedIdComparer<Guid, TId>>();
    }

    public static void HasRowVersion<TEntity>(this EntityTypeBuilder<TEntity> e)
        where TEntity : class
    {
        e.Property<uint>("xmin").IsRowVersion();
    }
}
