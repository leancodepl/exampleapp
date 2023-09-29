using Serilog;

namespace LeanCode.AuditLogs;

public class StubAuditLogStorage : IAuditLogStorage
{
    private readonly ILogger logger = Log.ForContext<StubAuditLogStorage>();

    public Task StoreEventAsync(
        EntityData entityChanged,
        string? actionName,
        DateTimeOffset dateOccurred,
        string? actorId,
        string? traceId,
        string? spanId,
        CancellationToken cancellationToken
    )
    {
        logger.Information(
            "StubAuditLog: Changes found {UserId} {ActionName} {Type} {State} {@PrimaryKey} {@EntryChanged} {DateOccurred}",
            actorId,
            actionName,
            entityChanged.Type,
            entityChanged.EntityState,
            entityChanged.Ids.Select(id => id.ToString()).ToList(),
            entityChanged.Changes,
            dateOccurred
        );

        return Task.CompletedTask;
    }
}
