using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ExampleApp.Examples.DataAccess.Blobs;

public class ServiceProviderLogoStorage : BaseBlobStorage
{
    protected override string ContainerName => "service-providers";
    protected override PublicAccessType DefaultAccessType => PublicAccessType.Blob;

    public ServiceProviderLogoStorage(BlobServiceClient client, BlobStorageDelegationKeyProvider keyProvider)
        : base(client, keyProvider) { }

    public Dictionary<string, string> GetRequiredUploadHeaders()
    {
        return new() { ["x-ms-blob-type"] = "blockblob" };
    }

    public virtual Task<Uri> StartLogoUploadAsync(CancellationToken cancellationToken)
    {
        return GetTemporaryUploadLinkAsync(Guid.NewGuid().ToString(), cancellationToken);
    }

    public virtual Task CommitLogoAsync(Uri logoUri, CancellationToken cancellationToken)
    {
        return CommitTemporaryUploadAsync(logoUri, cancellationToken);
    }

    public virtual Task DeleteLogoAsync(Uri logoUri, CancellationToken cancellationToken)
    {
        return DeleteAsync(logoUri, cancellationToken);
    }

    protected override bool IsValidBlobName(string blobName) => Guid.TryParse(blobName, out _);
}
