using ExampleApp.Examples.DataAccess.Blobs;

namespace ExampleApp.Examples.IntegrationTests.Helpers;

public class ServiceProviderLogoStorageMock : ServiceProviderLogoStorage
{
    public ServiceProviderLogoStorageMock()
        : base(null!, null!) { }

    public override Task<Uri> StartLogoUploadAsync(CancellationToken cancellationToken) =>
        Task.FromResult(new Uri("http://localhost"));

    public override Task CommitLogoAsync(Uri logoUri, CancellationToken cancellationToken) => Task.CompletedTask;

    public override bool IsValid(Uri uri) => true;

    public override Task DeleteLogoAsync(Uri logoUri, CancellationToken cancellationToken) => Task.CompletedTask;
}
