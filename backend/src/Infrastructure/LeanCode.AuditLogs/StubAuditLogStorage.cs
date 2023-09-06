using Serilog;

namespace LeanCode.AuditLogs;

public class StubAuditLogStorage : IAuditLogStorage
{
    private readonly ILogger logger = Log.ForContext<StubAuditLogStorage>();

    public Task StoreEventAsync(
        string objectType,
        IEnumerable<string> objectId,
        string? actionName,
        DateTimeOffset dateOccurred,
        string? actorId,
        string auditPayload,
        CancellationToken cancellationToken
    )
    {
        logger.Information(
            "StubAuditLog: Changes found {UserId} {ActionName} {Type} {@PrimaryKey} {@EntryChanged} {DateOccurred}",
            actorId,
            actionName,
            objectType,
            objectId,
            auditPayload,
            dateOccurred
        );

        return Task.CompletedTask;
    }
}
