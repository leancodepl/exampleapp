using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LeanCode.TimeProvider;
using Microsoft.Extensions.Caching.Memory;

namespace ExampleApp.Examples.DataAccess.Blobs;

public class BlobStorageDelegationKeyProvider
{
    private const string CacheKey = "user_delegation_key-token";
    private static readonly TimeSpan KeyLifetime = TimeSpan.FromDays(7);
    private static readonly TimeSpan MaxSasLifetime = TimeSpan.FromDays(2);
    private static readonly TimeSpan Skew = TimeSpan.FromMinutes(5);

    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<BlobStorageDelegationKeyProvider>();

    private readonly IMemoryCache memoryCache;
    private readonly BlobServiceClient client;

    public BlobStorageDelegationKeyProvider(IMemoryCache memoryCache, BlobServiceClient client)
    {
        this.memoryCache = memoryCache;
        this.client = client;
    }

    public Task<UserDelegationKey> GetKeyAsync(CancellationToken cancellationToken)
    {
        return memoryCache.GetOrCreateAsync(CacheKey, e => IssueKeyAsync(e, cancellationToken))!;
    }

    private async Task<UserDelegationKey> IssueKeyAsync(ICacheEntry entry, CancellationToken cancellationToken)
    {
        var now = Time.NowWithOffset;
        var key = await client.GetUserDelegationKeyAsync(now - Skew, now + KeyLifetime, cancellationToken);

        if (key?.Value is null)
        {
            logger.Warning("Cannot issue blob storage user delegation key");
            throw new InvalidOperationException("Cannot issue blob storage user delegation key.");
        }
        else
        {
            entry.SetAbsoluteExpiration(now + KeyLifetime - MaxSasLifetime - Skew);
            return key;
        }
    }
}
