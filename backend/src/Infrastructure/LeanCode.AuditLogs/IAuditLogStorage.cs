namespace LeanCode.AuditLogs;

public interface IAuditLogStorage
{
    public Task StoreEventAsync(
        EntityData changeTrackerAuditData,
        string actionName,
        DateTime dateOccurred,
        string? actorId,
        CancellationToken cancellationToken
    );
}
