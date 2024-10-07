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
    public Uri PromotionalBanner { get; private set; }
    public Uri ListItemPicture { get; private set; }
    public bool IsPromotionActive { get; private set; }
    public double Ratings { get; private set; }

    public string Address { get; private set; }
    public Location Location { get; private set; }

    DateTime IOptimisticConcurrency.DateModified { get; set; }

    private ServiceProvider()
    {
        Name = null!;
        Description = null!;
        PromotionalBanner = null!;
        ListItemPicture = null!;
        Address = null!;
        Location = null!;
    }

    public static ServiceProvider Create(
        string name,
        ServiceProviderType type,
        string description,
        Uri promotionalBanner,
        Uri listItemPicture,
        string address,
        Location location,
        double ratings
    )
    {
        return new ServiceProvider
        {
            Id = ServiceProviderId.New(),
            Name = name,
            Type = type,
            Description = description,
            PromotionalBanner = promotionalBanner,
            ListItemPicture = listItemPicture,
            Address = address,
            Location = location,
            Ratings = ratings,
        };
    }
}

public enum ServiceProviderType
{
    Hairdresser = 0,
    BarberShop,
    Groomer,
}
