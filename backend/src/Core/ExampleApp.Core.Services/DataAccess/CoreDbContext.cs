using ExampleApp.Core.Domain.Projects;
using LeanCode.DomainModels.EF;
using LeanCode.DomainModels.MassTransitRelay.Inbox;
using LeanCode.DomainModels.MassTransitRelay.Outbox;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Core.Services.DataAccess;

public class CoreDbContext : DbContext, IOutboxContext, IConsumedMessagesContext
{
    public DbContext Self => this;
    public DbSet<ConsumedMessage> ConsumedMessages => Set<ConsumedMessage>();
    public DbSet<RaisedEvent> RaisedEvents => Set<RaisedEvent>();

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Employee> Employees => Set<Employee>();

    public CoreDbContext(DbContextOptions<CoreDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("dbo");

        ConsumedMessage.Configure(builder);
        RaisedEvent.Configure(builder);

        builder.Entity<Employee>(e =>
        {
            e.HasKey(t => t.Id);

            e.Property(t => t.Id).IsTypedId();

            e.Property(t => t.Name);
            e.Property(t => t.Email);
        });

        builder.Entity<Project>(e =>
        {
            e.HasKey(t => t.Id);

            e.Property(t => t.Id).IsTypedId();

            e.Property(t => t.Name);

            e.OwnsMany(
                p => p.Assignments,
                inner =>
                {
                    inner.WithOwner(a => a.ParentProject).HasForeignKey(a => a.ParentProjectId);

                    inner.Property(a => a.Name);
                    inner.Property(a => a.Id).IsTypedId();
                    inner.Property(a => a.ParentProjectId).IsTypedId();
                    inner.Property(a => a.AssignedEmployeeId).IsTypedId();

                    inner.ToTable("Assignments");
                }
            );
        });
    }

    public Task CommitAsync(CancellationToken cancellationToken = default) => SaveChangesAsync(cancellationToken);
}
