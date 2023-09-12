using MassTransit;

namespace LeanCode.AuditLogs;

public class AuditLogsConsumer : IConsumer<AuditLogMessage>
{
    private readonly IAuditLogStorage auditLogStorage;

    public AuditLogsConsumer(IAuditLogStorage auditLogStorage)
    {
        this.auditLogStorage = auditLogStorage;
    }

    public Task Consume(ConsumeContext<AuditLogMessage> context)
    {
        var msg = context.Message;
        return auditLogStorage.StoreEventAsync(
            msg.EntitiesChanged,
            msg.ActionName,
            msg.DateOccurred,
            msg.ActorId,
            context.CancellationToken
        );
    }
}

public class AuditLogMessage
{
    public IReadOnlyList<EntityData> EntitiesChanged { get; set; } = null!;
    public string ActionName { get; set; } = null!;
    public DateTime DateOccurred { get; set; }
    public string? ActorId { get; set; }
}
