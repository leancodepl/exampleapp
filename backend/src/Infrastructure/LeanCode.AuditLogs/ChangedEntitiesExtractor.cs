using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace LeanCode.AuditLogs;

public static class ChangedEntitiesExtractor
{
    private static readonly JsonSerializerOptions Options =
        new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = false,
        };

    public static IReadOnlyList<EntityData> Extract(DbContext dbContext)
    {
        return dbContext.ChangeTracker
            .Entries()
            .Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached)
            .Select(
                e =>
                    new EntityData(
                        // TODO: FIXME, I fail with owned entities
                        e.Metadata
                            .FindPrimaryKey()!
                            .Properties.Select(
                                p =>
                                    JsonSerializer.Serialize(
                                        p.PropertyInfo?.GetMethod?.Invoke(e.Entity, null)
                                            ?? "Cannot extract key property",
                                        Options
                                    )
                            )
                            .ToList(),
                        e.Metadata.ClrType.ToString(),
                        JsonSerializer.Serialize(e.Entity, Options),
                        e.State.ToString()
                    )
            )
            .ToList();
    }
}
