using Serilog;

namespace LeanCode.AuditLogs;

public class StubAuditLogStorage : IAuditLogStorage
{
    private readonly ILogger logger = Log.ForContext<StubAuditLogStorage>();

    public Task StoreEventAsync(
        IReadOnlyList<EntityData> entitiesChanged,
        string? actionName,
        DateTime dateOccurred,
        string? actorId,
        CancellationToken cancellationToken
    )
    {
        foreach (var entityChanged in entitiesChanged)
        {
            logger.Information(
                "StubAuditLog: Changes found {UserId} {ActionName} {Type} {@PrimaryKey} {@EntryChanged} {DateOccurred}",
                actorId,
                actionName,
                entityChanged.Type,
                entityChanged.Ids.Select(id => id.ToString()).ToList(),
                entityChanged.Changes,
                dateOccurred
            );
        }

        return Task.CompletedTask;
    }
}
