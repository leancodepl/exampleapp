using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Serilog;

namespace LeanCode.AuditLogs;

public class AzureBlobAuditLogStorage : IAuditLogStorage
{
    private static readonly byte[] NewLineBytes = Encoding.UTF8.GetBytes("\n");
    private static readonly JsonSerializerOptions Options =
        new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = false,
        };

    private readonly BlobServiceClient client;
    private readonly AzureBlobAuditLogStorageConfiguration config;

    public AzureBlobAuditLogStorage(BlobServiceClient client, AzureBlobAuditLogStorageConfiguration config)
    {
        this.client = client;
        this.config = config;
    }

    public Task StoreEventAsync(
        IReadOnlyList<EntityData> entitiesChanged,
        string? actionName,
        DateTimeOffset dateOccurred,
        string? actorId,
        CancellationToken cancellationToken
    )
    {
        var logsToAppend = entitiesChanged.Select(
            entityChanged =>
                AppendLogToBlobAsync(
                    new()
                    {
                        EntityChanged = entityChanged,
                        ActionName = actionName,
                        ActorId = actorId,
                        DateOccurred = dateOccurred,
                    },
                    cancellationToken
                )
        );

        return Task.WhenAll(logsToAppend);
    }

    private async Task AppendLogToBlobAsync(EntryDataDTO entryData, CancellationToken cancellationToken)
    {
        var container = client.GetBlobContainerClient(config.AuditLogsContainer);
        var blob = container.GetAppendBlobClient(GetBlobName(entryData.EntityChanged));
        await blob.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        using var stream = new MemoryStream();
        JsonSerializer.Serialize(stream, entryData, Options);
        stream.Write(NewLineBytes, 0, NewLineBytes.Length);
        stream.Position = 0;
        await blob.AppendBlockAsync(stream, cancellationToken: cancellationToken);
    }

    private static string GetBlobName(EntityData entity)
    {
        return $"{entity.Type}/{string.Join("", entity.Ids)}";
    }
}

public record AzureBlobAuditLogStorageConfiguration(string AuditLogsContainer);

public class EntryDataDTO
{
    public EntityData EntityChanged { get; set; } = null!;
    public string? ActionName { get; set; }
    public string? ActorId { get; set; }
    public DateTimeOffset DateOccurred { get; set; }
}
