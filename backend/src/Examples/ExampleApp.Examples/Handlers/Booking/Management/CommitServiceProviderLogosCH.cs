using ExampleApp.Examples.DataAccess.Blobs;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Booking.Events;
using LeanCode.DomainModels.DataAccess;
using MassTransit;

namespace ExampleApp.Examples.Handlers.Booking.Management;

public class CommitServiceProviderLogosCH(
    IRepository<ServiceProvider, ServiceProviderId> serviceProviders,
    ServiceProviderLogoStorage logoStorage
) : IConsumer<ServiceProviderCreated>
{
    public async Task Consume(ConsumeContext<ServiceProviderCreated> context)
    {
        var msg = context.Message;
        var serviceProvider = await serviceProviders.FindAndEnsureExistsAsync(
            msg.ServiceProviderId,
            context.CancellationToken
        );

        await logoStorage.CommitLogoAsync(serviceProvider.CoverPhoto, context.CancellationToken);
        await logoStorage.CommitLogoAsync(serviceProvider.Thumbnail, context.CancellationToken);
    }
}
