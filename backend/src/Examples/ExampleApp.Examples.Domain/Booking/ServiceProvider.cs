using ExampleApp.Examples.Domain.Booking.Events;
using LeanCode.DomainModels.Ids;
using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples.Domain.Booking;

[TypedId(TypedIdFormat.PrefixedUlid, CustomPrefix = "serviceprovider")]
public readonly partial record struct ServiceProviderId;

public class ServiceProvider : IAggregateRoot<ServiceProviderId>
{
    public ServiceProviderId Id { get; private init; }

    public string Name { get; private set; }
    public ServiceProviderType Type { get; private set; }
    public string Description { get; private set; }
    public Uri CoverPhoto { get; private set; }
    public Uri Thumbnail { get; private set; }
    public bool IsPromotionActive { get; private set; }
    public double Ratings { get; private set; }

    public string Address { get; private set; }
    public Location Location { get; private set; }

    DateTime IOptimisticConcurrency.DateModified { get; set; }

    private ServiceProvider()
    {
        Name = null!;
        Description = null!;
        CoverPhoto = null!;
        Thumbnail = null!;
        Address = null!;
        Location = null!;
    }

    public static ServiceProvider Create(
        string name,
        ServiceProviderType type,
        string description,
        Uri coverPhoto,
        Uri thumbnail,
        string address,
        Location location,
        double ratings
    )
    {
        var serviceProvider = new ServiceProvider
        {
            Id = ServiceProviderId.New(),
            Name = name,
            Type = type,
            Description = description,
            CoverPhoto = coverPhoto,
            Thumbnail = thumbnail,
            Address = address,
            Location = location,
            Ratings = ratings,
        };

        DomainEvents.Raise(new ServiceProviderCreated(serviceProvider));

        return serviceProvider;
    }
}

public enum ServiceProviderType
{
    Hairdresser = 0,
    BarberShop = 1,
    Groomer = 2,
}
