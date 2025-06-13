using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using LeanCode.AppRating.DataAccess;
using LeanCode.Firebase.FCM;
using LeanCode.NotificationCenter.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.DataAccess;

public partial class ExamplesDbContext : IAppRatingStore<Guid>, INotificationsDbContext<Guid>
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Project> Projects => Set<Project>();

    public DbSet<ServiceProvider> ServiceProviders => Set<ServiceProvider>();
    public DbSet<CalendarDay> CalendarDays => Set<CalendarDay>();
    public DbSet<Timeslot> Timeslots => Set<Timeslot>();

    public DbSet<Reservation> Reservations => Set<Reservation>();

    public DbSet<PushNotificationTokenEntity<Guid>> PushNotificationTokens => Set<PushNotificationTokenEntity<Guid>>();
    public DbSet<AppRating<Guid>> AppRatings => Set<AppRating<Guid>>();

    public DbSet<UserData<Guid>> NotificationsUsers => Set<UserData<Guid>>();
    public DbSet<NotificationEntity<Guid>> Notifications => Set<NotificationEntity<Guid>>();

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
        modelBuilder.ConfigureAppRatingEntity<Guid>(LeanCode.AppRating.DataAccess.SqlDbType.PostgreSql);
        modelBuilder.ConfigureNotificationCenter<Guid>(LeanCode.NotificationCenter.DataAccess.SqlDbType.PostgreSql);

        modelBuilder.Entity<Employee>(e =>
        {
            e.HasKey(t => t.Id);

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

            e.HasRowVersion();
        });

        modelBuilder.Entity<ServiceProvider>(e =>
        {
            e.HasKey(t => t.Id);

            e.ComplexProperty(t => t.Location);

            e.HasRowVersion();
        });

        modelBuilder.Entity<CalendarDay>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasAlternateKey(t => new { t.ServiceProviderId, t.Date });

            e.HasMany(t => t.Timeslots).WithOne(t => t.CalendarDay);
            e.Navigation(t => t.Timeslots).AutoInclude();

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

            e.HasRowVersion();
        });
    }
}
