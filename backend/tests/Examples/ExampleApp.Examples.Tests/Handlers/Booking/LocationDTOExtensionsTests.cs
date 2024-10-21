using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Handlers.Booking;
using FluentAssertions;
using Xunit;

namespace ExampleApp.Examples.Tests.Handlers.Booking;

public class LocationDTOExtensionsTests
{
    [Fact]
    public void ToDTO_and_ToDomain_are_inverses()
    {
        var location = new Location(1.0, 2.0);

        location.ToDTO().ToDomain().Should().BeEquivalentTo(location);
    }

    [Fact]
    public void Coordinates_are_preserved_in_ToDTO()
    {
        var location = new Location(1.0, 2.0);

        var dto = location.ToDTO();

        dto.Latitude.Should().Be(location.Latitude);
        dto.Longitude.Should().Be(location.Longitude);
    }

    [Fact]
    public void Coordinates_are_preserved_in_ToDomain()
    {
        var dto = new LocationDTO(1.0, 2.0);

        var location = dto.ToDomain();

        location.Latitude.Should().Be(dto.Latitude);
        location.Longitude.Should().Be(dto.Longitude);
    }
}
