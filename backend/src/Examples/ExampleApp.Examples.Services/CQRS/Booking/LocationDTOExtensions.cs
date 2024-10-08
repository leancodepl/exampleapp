using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Domain.Booking;

namespace ExampleApp.Examples.Services.CQRS.Booking;

public static class LocationDTOExtensions
{
    public static LocationDTO ToDTO(this Location l) => new LocationDTO(l.Latitude, l.Longitude);

    public static Location ToDomain(this LocationDTO l) => new Location(l.Latitude, l.Longitude);
}
