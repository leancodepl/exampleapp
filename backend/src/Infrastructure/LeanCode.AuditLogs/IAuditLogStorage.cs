namespace LeanCode.AuditLogs;

public interface IAuditLogStorage
{
    public Task StoreEventAsync(
        EntityData changeTrackerAuditData,
        string actionName,
        DateTimeOffset dateOccurred,
        string? actorId,
        string? traceId,
        string? spanId,
        CancellationToken cancellationToken
    );
}
