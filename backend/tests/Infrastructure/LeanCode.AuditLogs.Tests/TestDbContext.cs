using System.Data.Common;
using LeanCode.DomainModels.Model;
using LeanCode.DomainModels.EF;
using Microsoft.EntityFrameworkCore;

namespace LeanCode.AuditLogs.Tests;

public class TestDbContext : DbContext
{
    private readonly DbConnection connection;

    public DbSet<TestEntity> TestEntities => Set<TestEntity>();

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
        connection = Database.GetDbConnection();
    }

    public static async Task<TestDbContext> CreateInMemory()
    {
        var context = new TestDbContext(
            new DbContextOptionsBuilder<TestDbContext>().UseSqlite("Filename=:memory:").Options
        );
        await context.connection.OpenAsync();
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestEntity>(e =>
        {
            e.HasKey(t => t.Id);

            e.OwnsMany(
                t => t.OwnedEntities,
                cfg =>
                {
                    cfg.HasKey(e => e.SomeInt);
                    cfg.Property(e => e.SomeString).HasMaxLength(100);
                }
            );

            e.HasMany(e => e.IncludedEntities).WithOne(e => e.TestEntity).HasForeignKey(e => e.TestEntityId);

            e.IsOptimisticConcurrent(addRowVersion: false);
        });

        modelBuilder.Entity<IncludedEntity>(e =>
        {
            e.HasKey(e => new { e.TestEntityId, e.SomeInt });
            e.Property(e => e.SomeString).HasMaxLength(100);
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await connection.DisposeAsync();
        await base.DisposeAsync();
    }
}

public class TestEntity : IAggregateRoot<string>
{
    private readonly List<OwnedEntity> ownedEntities = [];
    private readonly List<IncludedEntity> includedEntities = [];
    public string Id { get; set; }
    public string SomeString { get; set; }

    public IReadOnlyList<OwnedEntity> OwnedEntities => ownedEntities;
    public IReadOnlyList<IncludedEntity> IncludedEntities => includedEntities;


    DateTime IOptimisticConcurrency.DateModified { get; set; }

    public static TestEntity Create(string id)
    {
        return new() { Id = id, SomeString = "initial_value", };
    }

    public void SetSomeString(string someString)
    {
        SomeString = someString;
    }

    public void AddOwnedEntities()
    {
        foreach (var i in Enumerable.Range(20, 5))
        {
            ownedEntities.Add(new OwnedEntity(i, i.ToString()));
        }
    }

    public void AddIncludedEntities()
    {
        foreach (var i in Enumerable.Range(20, 5))
        {
            includedEntities.Add(new IncludedEntity(this, i, i.ToString()));
        }
    }

    public void UpdateOwnedEntities()
    {
        foreach (var e in ownedEntities)
        {
            e.Update((e.SomeInt + 100).ToString());
        }
    }

    public void UpdateIncludedEntities()
    {
        foreach (var e in includedEntities)
        {
            e.Update((e.SomeInt + 100).ToString());
        }
    }
}

public class OwnedEntity
{
    public int SomeInt { get; private set; }
    public string SomeString { get; private set; } = null!;

    private OwnedEntity() { }

    public OwnedEntity(int someInt, string someString)
    {
        SomeInt = someInt;
        SomeString = someString;
    }

    public void Update(string someString)
    {
        SomeString = someString;
    }
}

public class IncludedEntity
{
    public TestEntity TestEntity { get; private init; } = null!;
    public string TestEntityId { get; private init; } = null!;
    public int SomeInt { get; private set; }
    public string SomeString { get; private set; } = null!;

    private IncludedEntity() { }

    public IncludedEntity(TestEntity testEntity, int someInt, string someString)
    {
        TestEntity = testEntity;
        TestEntityId = testEntity.Id;
        SomeInt = someInt;
        SomeString = someString;
    }

    public void Update(string someString)
    {
        SomeString = someString;
    }
}
