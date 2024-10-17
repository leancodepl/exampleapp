using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Domain.Booking;

namespace ExampleApp.Examples.Services.Handlers.Booking;

public static class LocationDTOExtensions
{
    public static LocationDTO ToDTO(this Location l) => new(l.Latitude, l.Longitude);

    public static Location ToDomain(this LocationDTO l) => new(l.Latitude, l.Longitude);
}
