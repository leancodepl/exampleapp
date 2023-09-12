using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace LeanCode.AuditLogs;

public static class ChangedEntitiesExtractor
{
    public static IReadOnlyList<EntityData> Extract(DbContext dbContext)
    {
        return dbContext.ChangeTracker
            .Entries()
            .Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached)
            .Select(
                e =>
                    new EntityData
                    {
                        // TODO: FIXME, I fail with owned entities
                        Ids = e.Metadata
                            .FindPrimaryKey()!
                            .Properties.Select(
                                p =>
                                    JsonSerializer.Serialize(
                                        p.PropertyInfo?.GetMethod?.Invoke(e.Entity, null)
                                            ?? "Cannot extract key property"
                                    )
                            )
                            .ToList(),
                        Type = e.Metadata.ClrType.ToString(),
                        Changes = JsonSerializer.Serialize(e.Entity),
                        EntityState = e.State.ToString(),
                    }
            )
            .ToList();
    }
}
