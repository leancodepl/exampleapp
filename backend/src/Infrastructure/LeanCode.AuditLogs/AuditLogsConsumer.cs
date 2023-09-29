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
            msg.EntityChanged,
            msg.ActionName,
            msg.DateOccurred,
            msg.ActorId,
            msg.TraceId,
            msg.SpanId,
            context.CancellationToken
        );
    }
}

public record AuditLogMessage(
    EntityData EntityChanged,
    string ActionName,
    DateTimeOffset DateOccurred,
    string? ActorId,
    string? TraceId,
    string? SpanId
);
