using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ExampleApp.Examples.DataAccess.Blobs;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace ExampleApp.Examples.Tests.DataAccess.Blobs;

public class BaseBlobStorageTests
{
    private readonly BlobServiceClientMock clientMock;
    private readonly BaseBlobStorageMock storage;

    public BaseBlobStorageTests()
    {
        clientMock = new();
        storage = new(clientMock);
    }

    [Fact]
    public void Validates_links_to_blobs_in_the_container()
    {
        storage.IsValid(new("https://myaccount.blob.core.windows.net/container/myblob")).Should().BeTrue();
    }

    [Fact]
    public void Links_with_wrong_properties_are_invalid()
    {
        storage
            .IsValid(new("https://myaccount.blob.core.windows.net/container/invalid-blob"))
            .Should()
            .BeFalse("blob name is invalid");

        storage
            .IsValid(new("https://myaccount.blob.core.windows.net/invalidcontainer/myblob"))
            .Should()
            .BeFalse("container name is invalid");

        storage
            .IsValid(new("https://invalidaccount.blob.core.windows.net/container/myblob"))
            .Should()
            .BeFalse("account is wrong");

        storage.IsValid(new("https://leancode.co")).Should().BeFalse("this is not a blob URI");
    }

    [Fact]
    public void PrepareForStorage_strips_query_string()
    {
        storage
            .PrepareForStorage(new("https://myaccount.blob.core.windows.net:443/container/myblob?this=is&a=query"))
            .Should()
            .Be(new Uri("https://myaccount.blob.core.windows.net/container/myblob"));
    }

    [Fact]
    public async Task Download_link_by_filename_points_to_correct_account_and_has_SAS_query_params()
    {
        var uri = await storage.GetDownloadLinkAsync("filename");
        var builder = new BlobUriBuilder(uri);

        builder
            .Should()
            .BeEquivalentTo(
                new
                {
                    AccountName = "myaccount",
                    BlobContainerName = "container",
                    BlobName = "filename",
                }
            );
        builder.Sas.Should().NotBeNull();
        builder.Sas.Permissions.Should().Be("r");
        builder.Sas.Signature.Should().NotBeNull();
    }

    [Fact]
    public async Task Download_link_by_uri_points_to_correct_account_and_has_SAS_query_params()
    {
        var uri = await storage.GetDownloadLinkAsync(
            new Uri("https://myaccount.blob.core.windows.net/container/myblob")
        );
        var builder = new BlobUriBuilder(uri);

        builder
            .Should()
            .BeEquivalentTo(
                new
                {
                    AccountName = "myaccount",
                    BlobContainerName = "container",
                    BlobName = "myblob",
                }
            );
        builder.Sas.Should().NotBeNull();
        builder.Sas.Permissions.Should().Be("r");
        builder.Sas.Signature.Should().NotBeNull();
    }

    [Fact]
    public async Task Permanent_upload_link_points_to_correct_account_and_has_SAS_query_params()
    {
        var uri = await storage.GetTemporaryUploadLinkAsync("filename");
        var builder = new BlobUriBuilder(uri);

        builder
            .Should()
            .BeEquivalentTo(
                new
                {
                    AccountName = "myaccount",
                    BlobContainerName = "container",
                    BlobName = "filename",
                }
            );
        builder.Sas.Should().NotBeNull();
        builder.Sas.Permissions.Should().NotContain("c").And.Contain("w").And.Contain("r");
        builder.Sas.Signature.Should().NotBeNull();
    }

    [Fact]
    public async Task Temporary_upload_link_points_to_correct_account_and_has_SAS_query_params()
    {
        var uri = await storage.GetPermanentUploadLinkAsync("filename");
        var builder = new BlobUriBuilder(uri);

        builder
            .Should()
            .BeEquivalentTo(
                new
                {
                    AccountName = "myaccount",
                    BlobContainerName = "container",
                    BlobName = "filename",
                }
            );
        builder.Sas.Should().NotBeNull();
        builder.Sas.Permissions.Should().Contain("c").And.Contain("w").And.Contain("r");
        builder.Sas.Signature.Should().NotBeNull();
    }
}

public class BaseBlobStorageMock : BaseBlobStorage
{
    public string FakeContainerName { get; set; } = "container";
    public string AllowedBlobName { get; set; } = "myblob";
    public BlobServiceClientMock ClientMock => (BlobServiceClientMock)Client;

    protected override string ContainerName => FakeContainerName;
    protected override PublicAccessType DefaultAccessType => PublicAccessType.Blob;

    protected override bool IsValidBlobName(string blobName) => AllowedBlobName == blobName;

    public BaseBlobStorageMock(BlobServiceClientMock mock)
        : base(mock, new(new MemoryCache(new MemoryCacheOptions()), mock)) { }

    public Task<Uri> GetDownloadLinkAsync(string filename)
    {
        return GetDownloadLinkAsync(filename, null, default);
    }

    public Task<Uri> GetDownloadLinkAsync(Uri uri)
    {
        return GetDownloadLinkAsync(uri, null, default);
    }

    public Task<Uri> GetPermanentUploadLinkAsync(string filename)
    {
        return GetPermanentUploadLinkAsync(filename, default);
    }

    public Task<Uri> GetTemporaryUploadLinkAsync(string filename)
    {
        return GetTemporaryUploadLinkAsync(filename, default);
    }
}

public class BlobServiceClientMock : BlobServiceClient
{
    internal const string ConnectionString =
        "DefaultEndpointsProtocol=http;AccountName=myaccount;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/myaccount;";

    public BlobServiceClientMock()
        : base(ConnectionString) { }

    public override BlobContainerClient GetBlobContainerClient(string blobContainerName) =>
        new BlobContainerClientMock(blobContainerName);
}

public class BlobContainerClientMock : BlobContainerClient
{
    public BlobContainerClientMock(string containerName)
        : base(BlobServiceClientMock.ConnectionString, containerName) { }

    public override Task<Response<BlobContainerInfo>> CreateIfNotExistsAsync(
        PublicAccessType publicAccessType = PublicAccessType.None,
        IDictionary<string, string>? metadata = null,
        BlobContainerEncryptionScopeOptions? encryptionScopeOptions = null,
        CancellationToken cancellationToken = default
    ) => Task.FromResult((Response<BlobContainerInfo>)null!);

    public override BlobClient GetBlobClient(string blobName) => new BlobClientMock(Name, blobName);
}

public class BlobClientMock : BlobClient
{
    public BlobClientMock(string containerName, string blobName)
        : base(BlobServiceClientMock.ConnectionString, containerName, blobName) { }

    public override Task<Response<BlobContentInfo>> UploadAsync(
        Stream content,
        BlobUploadOptions options,
        CancellationToken cancellationToken = default
    ) => Task.FromResult((Response<BlobContentInfo>)null!);
}
