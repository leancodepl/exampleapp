using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.ServiceProviders;

/// <summary>
/// The query will return details about service provider and all available timeslots from <see cref="CurrentTime"/> to
/// +X days (configurable on query level).
/// </summary>
[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
public class ServiceProviderDetails : IQuery<ServiceProviderDetailsDTO?>
{
    public string ServiceProviderId { get; set; }
    public DateTimeOffset CurrentTime { get; set; }
}

public class ServiceProviderDetailsDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ServiceProviderTypeDTO Type { get; set; }
    public string Address { get; set; }
    public LocationDTO Location { get; set; }
    public bool IsPromotionActive { get; set; }

    public Uri PromotionalBanner { get; set; }
    public Uri ListItemPicture { get; set; }

    public List<AvailableTimeslotDTO> AvailableTimeslots { get; set; }
}

public class AvailableTimeslotDTO
{
    public string Id { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public MoneyDTO Price { get; set; }
}