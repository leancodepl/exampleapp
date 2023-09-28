using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Serilog;

namespace LeanCode.AuditLogs;

public class AzureBlobAuditLogStorage : IAuditLogStorage
{
    private readonly ILogger logger = Log.ForContext<AzureBlobAuditLogStorage>();

    private static ReadOnlySpan<byte> NewLineBytes => "\n"u8;
    private static readonly JsonSerializerOptions Options =
        new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = false,
        };

    private readonly BlobServiceClient blobClient;
    private readonly TableServiceClient tableClient;
    private readonly AzureBlobAuditLogStorageConfiguration config;

    public AzureBlobAuditLogStorage(
        BlobServiceClient blobClient,
        TableServiceClient tableClient,
        AzureBlobAuditLogStorageConfiguration config
    )
    {
        this.blobClient = blobClient;
        this.tableClient = tableClient;
        this.config = config;
    }

    public Task StoreEventAsync(
        IReadOnlyList<EntityData> entitiesChanged,
        string? actionName,
        DateTimeOffset dateOccurred,
        string? actorId,
        string? traceId,
        string? spanId,
        CancellationToken cancellationToken
    )
    {
        var logsToAppend = entitiesChanged.Select(
            entityChanged =>
                HandleSingleLogAsync(
                    new()
                    {
                        EntityChanged = entityChanged,
                        ActionName = actionName,
                        ActorId = actorId,
                        DateOccurred = dateOccurred,
                        TraceId = traceId,
                        SpanId = spanId,
                    },
                    cancellationToken
                )
        );

        return Task.WhenAll(logsToAppend);
    }

    private async Task HandleSingleLogAsync(EntryDataDTO entryData, CancellationToken cancellationToken)
    {
        try
        {
            await AppendLogToBlobAsync(entryData, cancellationToken);
        }
        catch (RequestFailedException e)
        {
            if (e.ErrorCode == BlobErrorCode.BlockCountExceedsLimit)
            {
                await BumpSuffixInTableAsync(entryData, cancellationToken);
                await AppendLogToBlobAsync(entryData, cancellationToken);
            }
            else
            {
                throw;
            }
        }
    }

    private async Task AppendLogToBlobAsync(EntryDataDTO entryData, CancellationToken cancellationToken)
    {
        var container = blobClient.GetBlobContainerClient(config.AuditLogsContainer);
        var table = tableClient.GetTableClient(config.AuditLogsTable);
        var suffix = await GetSuffixAsync(entryData, table, cancellationToken);

        var blobName = GetBlobName(entryData.EntityChanged, suffix);

        var blob = container.GetAppendBlobClient(blobName);
        await blob.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        using var stream = new MemoryStream();
        JsonSerializer.Serialize(stream, entryData, Options);
        stream.Write(NewLineBytes.ToArray(), 0, NewLineBytes.Length);
        stream.Position = 0;
        await blob.AppendBlockAsync(stream, cancellationToken: cancellationToken);

        logger.Verbose("Log append to the blob {BlobName}", blobName);
    }

    private static async Task<int> GetSuffixAsync(
        EntryDataDTO entryData,
        TableClient table,
        CancellationToken cancellationToken
    )
    {
        var res = await table.GetEntityIfExistsAsync<TableEntity>(
            entryData.EntityChanged.Type,
            string.Join("", entryData.EntityChanged.Ids),
            cancellationToken: cancellationToken
        );
        if (!res.HasValue)
        {
            var entity = new TableEntity(entryData.EntityChanged.Type, string.Join("", entryData.EntityChanged.Ids))
            {
                ["Suffix"] = 0,
            };
            await table.AddEntityAsync(entity, cancellationToken: cancellationToken);

            return (int)entity["Suffix"];
        }
        else
        {
            return (int)res.Value["Suffix"];
        }
    }

    private async Task BumpSuffixInTableAsync(EntryDataDTO entryData, CancellationToken cancellationToken)
    {
        var table = tableClient.GetTableClient(config.AuditLogsTable);
        var res = await table.GetEntityAsync<TableEntity>(
            entryData.EntityChanged.Type,
            string.Join("", entryData.EntityChanged.Ids),
            cancellationToken: cancellationToken
        );
        var entity = res.Value;
        entity["Suffix"] = (int)entity["Suffix"] + 1;
        await table.UpdateEntityAsync(entity, entity.ETag, cancellationToken: cancellationToken);
    }

    private static string GetBlobName(EntityData entity, int suffix)
    {
        return $"{entity.Type}/{string.Join("", entity.Ids)}.{suffix}";
    }
}

public record AzureBlobAuditLogStorageConfiguration(string AuditLogsContainer, string AuditLogsTable);

public class EntryDataDTO
{
    public EntityData EntityChanged { get; set; } = null!;
    public string? ActionName { get; set; }
    public string? ActorId { get; set; }
    public DateTimeOffset DateOccurred { get; set; }
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }
}
