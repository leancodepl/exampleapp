using Microsoft.EntityFrameworkCore;

namespace LeanCode.AuditLogs;

public static class ChangedEntitiesExtractor<TDbContext>
    where TDbContext : DbContext
{
    public static IEnumerable<AuditData> Extract(TDbContext dbContext)
    {
        return dbContext.ChangeTracker
            .Entries()
            .Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached)
            .Select(
                e =>
                    new AuditData
                    {
                        Ids =
                            e.Metadata
                                .FindPrimaryKey()
                                ?.Properties.Select(
                                    p =>
                                        p.PropertyInfo?.GetMethod?.Invoke(e.Entity, null)?.ToString()
                                        ?? throw new NullReferenceException()
                                ) ?? throw new NullReferenceException(),
                        Type = e.Metadata.ClrType.ToString(),
                        Changes = e.DebugView.LongView
                    }
            );
    }

    public class AuditData
    {
        public IEnumerable<string> Ids { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Changes { get; set; } = null!;
    }
}
