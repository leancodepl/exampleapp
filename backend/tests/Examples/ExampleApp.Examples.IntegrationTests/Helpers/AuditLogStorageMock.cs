using LeanCode.AuditLogs;

namespace ExampleApp.Examples.IntegrationTests.Helpers;

public class AuditLogStorageMock : IAuditLogStorage
{
    public Task StoreEventAsync(AuditLogMessage auditLogMessage, CancellationToken cancellationToken) =>
        Task.CompletedTask;
}
