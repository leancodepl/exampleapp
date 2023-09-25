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

public record AuditLogMessage(
    IReadOnlyList<EntityData> EntitiesChanged,
    string ActionName,
    DateTime DateOccurred,
    string? ActorId
);
