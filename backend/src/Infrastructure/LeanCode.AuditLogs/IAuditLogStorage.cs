namespace LeanCode.AuditLogs;

public interface IAuditLogStorage
{
    public Task StoreEventAsync(
        IReadOnlyList<EntityData> changeTrackerAuditData,
        string actionName,
        DateTimeOffset dateOccurred,
        string? actorId,
        CancellationToken cancellationToken
    );
}
