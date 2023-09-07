using Microsoft.EntityFrameworkCore;

namespace LeanCode.AuditLogs;

public static class ChangedEntitiesExtractor<TDbContext>
    where TDbContext : DbContext
{
    public static IEnumerable<EntityData> Extract(TDbContext dbContext)
    {
        return dbContext.ChangeTracker
            .Entries()
            .Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached)
            .Select(
                e =>
                    new EntityData
                    {
                        Ids = e.Metadata
                            .FindPrimaryKey()!
                            .Properties.Select(p => p.PropertyInfo!.GetMethod!.Invoke(e.Entity, null)!.ToString())
                            .Cast<string>(),
                        Type = e.Metadata.ClrType.ToString(),
                        Changes = e.DebugView.LongView
                    }
            );
    }
}
