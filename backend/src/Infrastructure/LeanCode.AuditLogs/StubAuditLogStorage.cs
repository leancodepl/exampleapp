using Serilog;

namespace LeanCode.AuditLogs;

public class StubAuditLogStorage : IAuditLogStorage
{
    private readonly ILogger logger = Log.ForContext<StubAuditLogStorage>();

    public Task StoreEventAsync(
        EntityData entityChanged,
        string? actionName,
        DateTime dateOccurred,
        string? actorId,
        CancellationToken cancellationToken
    )
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

        return Task.CompletedTask;
    }
}
