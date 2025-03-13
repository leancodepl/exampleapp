using ExampleApp.Examples.Contracts.Booking.Management;
using ExampleApp.Examples.DataAccess.Blobs;
using LeanCode.CQRS.Execution;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Handlers.Booking.Management;

public class ServiceProviderLogoUploadLinkQH
    : IQueryHandler<ServiceProviderLogoUploadLink, ServiceProviderLogoUploadLinkDTO>
{
    private readonly ServiceProviderLogoStorage logoStorage;

    public ServiceProviderLogoUploadLinkQH(ServiceProviderLogoStorage logoStorage)
    {
        this.logoStorage = logoStorage;
    }

    public async Task<ServiceProviderLogoUploadLinkDTO> ExecuteAsync(
        HttpContext context,
        ServiceProviderLogoUploadLink query
    )
    {
        return new()
        {
            Link = await logoStorage.StartLogoUploadAsync(context.RequestAborted),
            RequiredHeaders = logoStorage.GetRequiredUploadHeaders(),
        };
    }
}
