namespace LeanCode.AuditLogs;

public interface IAuditLogStorage
{
    public Task StoreEventAsync(
        string objectType,
        IEnumerable<string> objectId,
        string actionName,
        DateTimeOffset dateOccurred,
        string? actorId,
        string auditPayload,
        CancellationToken cancellationToken
    );
}
