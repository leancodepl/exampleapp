using System.Diagnostics.CodeAnalysis;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace ExampleApp.Examples.DataAccess.Blobs;

public abstract class BaseBlobStorage
{
    private const string DeleteTagName = "ToDelete";
    private const string DeleteValue = "1";

    private readonly BlobStorageDelegationKeyProvider keyProvider;

    protected BlobServiceClient Client { get; }
    protected abstract string ContainerName { get; }
    protected abstract PublicAccessType DefaultAccessType { get; }

    protected BaseBlobStorage(BlobServiceClient client, BlobStorageDelegationKeyProvider keyProvider)
    {
        Client = client;
        this.keyProvider = keyProvider;
    }

    public virtual bool IsValid(Uri uri)
    {
        var blobBuilder = new BlobUriBuilder(uri, false);
        return IsValid(blobBuilder);
    }

    [return: NotNullIfNotNull(nameof(uri))]
    public virtual Uri? PrepareForStorage(Uri? uri)
    {
        if (uri is null)
        {
            return null;
        }
        else
        {
            ValidateUri(uri);
            return new UriBuilder(uri)
            {
                Query = null,
                Port = -1,
                Fragment = null,
            }.Uri;
        }
    }

    protected abstract bool IsValidBlobName(string blobName);

    protected async Task<Uri> GetTemporaryUploadLinkAsync(string filename, CancellationToken cancellationToken)
    {
        var blob = await GetBlobClientAsync(filename, cancellationToken);

        await blob.UploadAsync(
            Stream.Null,
            new BlobUploadOptions { Tags = new Dictionary<string, string> { [DeleteTagName] = DeleteValue } },
            cancellationToken
        );

        return await GenerateBlobSasAsync(
            blob,
            BlobSasPermissions.Write | BlobSasPermissions.Read,
            null,
            cancellationToken
        );
    }

    protected async Task CommitTemporaryUploadAsync(Uri uri, CancellationToken cancellationToken)
    {
        ValidateUri(uri);

        var blobUri = new BlobUriBuilder(uri);
        var container = Client.GetBlobContainerClient(ContainerName);
        var blob = container.GetBlobClient(blobUri.BlobName);

        await blob.SetTagsAsync(new Dictionary<string, string>(), cancellationToken: cancellationToken);
    }

    protected async Task<Uri> GetPermanentUploadLinkAsync(string filename, CancellationToken cancellationToken)
    {
        var blob = await GetBlobClientAsync(filename, cancellationToken);

        return await GenerateBlobSasAsync(
            blob,
            BlobSasPermissions.Create | BlobSasPermissions.Write | BlobSasPermissions.Read,
            null,
            cancellationToken
        );
    }

    protected async Task DeleteAsync(Uri uri, CancellationToken cancellationToken)
    {
        ValidateUri(uri);

        var blobUri = new BlobUriBuilder(uri);
        var container = Client.GetBlobContainerClient(ContainerName);
        var blob = container.GetBlobClient(blobUri.BlobName);
        await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
    }

    protected async Task<Uri> GetDownloadLinkAsync(
        string filename,
        string? contentDisposition = null,
        CancellationToken cancellationToken = default
    )
    {
        var blob = GetBlobClient(filename);
        return await GenerateBlobSasAsync(blob, BlobSasPermissions.Read, contentDisposition, cancellationToken);
    }

    protected async Task<Uri> GetDownloadLinkAsync(
        Uri uri,
        string? contentDisposition = null,
        CancellationToken cancellationToken = default
    )
    {
        var blob = GetBlobClient(uri);
        return await GenerateBlobSasAsync(blob, BlobSasPermissions.Read, contentDisposition, cancellationToken);
    }

    protected BlobClient GetBlobClient(string filename)
    {
        var container = Client.GetBlobContainerClient(ContainerName);
        return container.GetBlobClient(filename);
    }

    protected BlobClient GetBlobClient(Uri uri)
    {
        var parsed = Parse(uri);
        var container = Client.GetBlobContainerClient(ContainerName);
        return container.GetBlobClient(parsed.BlobName);
    }

    protected async Task<BlobClient> GetBlobClientAsync(string filename, CancellationToken cancellationToken = default)
    {
        var container = Client.GetBlobContainerClient(ContainerName);
        await container.CreateIfNotExistsAsync(DefaultAccessType, cancellationToken: cancellationToken);
        return container.GetBlobClient(filename);
    }

    protected async Task<Uri> GenerateBlobSasAsync(
        BlobClient blob,
        BlobSasPermissions permissions,
        string? contentDisposition = null,
        CancellationToken cancellationToken = default
    )
    {
        var sasBuilder = new BlobSasBuilder(permissions, DateTimeOffset.UtcNow.AddDays(2))
        {
            BlobName = blob.Name,
            BlobContainerName = blob.BlobContainerName,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow.AddMinutes(-3),
            ContentDisposition = contentDisposition,
        };

        if (Client.CanGenerateAccountSasUri)
        {
            return blob.GenerateSasUri(sasBuilder);
        }
        else
        {
            var userDelegationKey = await keyProvider.GetKeyAsync(cancellationToken);
            return new BlobUriBuilder(blob.Uri)
            {
                Sas = sasBuilder.ToSasQueryParameters(userDelegationKey, Client.AccountName),
            }.ToUri();
        }
    }

    protected void ValidateUri(Uri uri)
    {
        if (!IsValid(uri))
        {
            throw new ArgumentException("The URI is not a valid logo URI.", nameof(uri));
        }
    }

    protected void ValidateUri(BlobUriBuilder uri)
    {
        if (!IsValid(uri))
        {
            throw new ArgumentException("The URI is not a valid logo URI.", nameof(uri));
        }
    }

    protected BlobUriBuilder Parse(Uri uri)
    {
        var builder = new BlobUriBuilder(uri);
        ValidateUri(builder);
        return builder;
    }

    private bool IsValid(BlobUriBuilder blobBuilder)
    {
        return blobBuilder.AccountName == Client.AccountName
            && blobBuilder.BlobContainerName == ContainerName
            && IsValidBlobName(blobBuilder.BlobName);
    }
}
