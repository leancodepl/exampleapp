using ExampleApp.Examples.Domain.Booking.Events;
using LeanCode.DomainModels.Ids;
using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples.Domain.Booking;

[TypedId(TypedIdFormat.PrefixedUlid, CustomPrefix = "serviceprovider")]
public readonly partial record struct ServiceProviderId;

public class ServiceProvider : IAggregateRoot<ServiceProviderId>
{
    private readonly List<Timeslot> timeslots = new();

    public ServiceProviderId Id { get; private init; }

    public string Name { get; private set; }
    public ServiceProviderType Type { get; private set; }
    public string Description { get; private set; }
    public Uri PromotionalBanner { get; private set; }
    public Uri ListItemPicture { get; private set; }
    public bool IsPromotionActive { get; private set; }

    public string Address { get; private set; }
    public Location Location { get; private set; }

    public IReadOnlyList<Timeslot> Timeslots => timeslots;

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
        Location location
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
        };
    }

    public bool CanAddTimeslotAt(DateOnly date, TimeOnly startTime, TimeOnly endTime) =>
        !timeslots.Any(ts => ts.Date == date && (ts.StartTime < endTime && ts.EndTime > startTime));

    public void AddTimeslot(DateOnly date, TimeOnly startTime, TimeOnly endTime, Money price)
    {
        if (!CanAddTimeslotAt(date, startTime, endTime))
        {
            throw new InvalidOperationException("The new timeslot overlaps with an existing timeslot.");
        }

        var newTimeslot = Timeslot.Create(this, date, startTime, endTime, price);
        timeslots.Add(newTimeslot);
    }

    public void ReserveTimeslot(TimeslotId timeslotId)
    {
        var removedSlots = timeslots.RemoveAll(t => t.Id == timeslotId);
        if (removedSlots == 1)
        {
            DomainEvents.Raise(new TimeslotReserved(Id, timeslotId));
        }
        else
        {
            DomainEvents.Raise(new TimeslotUnavailable(Id, timeslotId));
        }
    }
}

public enum ServiceProviderType
{
    Hairdresser = 0,
    BarberShop,
    Groomer,
}
