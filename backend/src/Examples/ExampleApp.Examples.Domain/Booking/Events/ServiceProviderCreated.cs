using System.Text.Json.Serialization;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;

namespace ExampleApp.Examples.Domain.Booking.Events;

public class ServiceProviderCreated : IDomainEvent
{
    public Guid Id { get; }
    public DateTime DateOccurred { get; }

    public ServiceProviderId ServiceProviderId { get; }

    public ServiceProviderCreated(ServiceProvider serviceProvider)
    {
        Id = Guid.NewGuid();
        DateOccurred = Time.UtcNow;

        ServiceProviderId = serviceProvider.Id;
    }

    [JsonConstructor]
    public ServiceProviderCreated(Guid id, DateTime dateOccurred, ServiceProviderId serviceProviderId)
    {
        Id = id;
        DateOccurred = dateOccurred;
        ServiceProviderId = serviceProviderId;
    }
}
