using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Contracts.Booking.Management;
using ExampleApp.Examples.Contracts.Booking.ServiceProviders;
using ExampleApp.Examples.IntegrationTests.Helpers;
using FluentAssertions;

namespace ExampleApp.Examples.IntegrationTests.Booking;

public abstract class BookingTestsBase : TestsBase<AuthenticatedExampleAppTestApp>
{
    protected const string Currency = "PLN";

    protected async Task<List<TimeslotDTO>> ListTimeslotsAsync(
        string spId,
        DateOnly? date = null,
        CancellationToken cancellationToken = default
    )
    {
        var filterDate = date ?? new(2024, 10, 23);

        var details = await App.Query.GetAsync(
            new ServiceProviderDetails { ServiceProviderId = spId, CalendarDate = filterDate },
            cancellationToken
        );
        details.Should().NotBeNull();
        return details!.Timeslots;
    }

    protected async Task<TimeslotDTO> AddTimeslotAsync(
        string spId,
        int hour = 14,
        CancellationToken cancellationToken = default
    )
    {
        var date = new DateOnly(2024, 10, 23);
        var from = new TimeOnly(hour, 0);
        var addTimeslot = new AddTimeslot
        {
            ServiceProviderId = spId,
            Date = date,
            StartTime = from,
            EndTime = new(from.Hour + 1, 0),
            Price = new MoneyDTO((int)(10m * 100), Currency),
        };

        await App.Command.RunSuccessAsync(addTimeslot);

        var details = await App.Query.GetAsync(
            new ServiceProviderDetails { ServiceProviderId = spId, CalendarDate = date },
            cancellationToken
        );
        return details!.Timeslots.Should().ContainSingle(t => t.StartTime == from).Which;
    }

    protected async Task AddTimeslotAsync(
        string spId,
        DateOnly date,
        TimeOnly from,
        TimeOnly to,
        decimal price,
        CancellationToken cancellationToken = default
    )
    {
        var addTimeslot = new AddTimeslot
        {
            ServiceProviderId = spId,
            Date = date,
            StartTime = from,
            EndTime = to,
            Price = new MoneyDTO((int)(price * 100), Currency),
        };

        await App.Command.RunSuccessAsync(addTimeslot);
    }

    protected async Task<string> CreateServiceProviderAsync(
        string? name = null,
        ServiceProviderTypeDTO type = ServiceProviderTypeDTO.Hairdresser,
        double ratings = 5.0,
        CancellationToken cancellationToken = default
    )
    {
        name ??= Guid.NewGuid().ToString();
        var createServiceProvider = new CreateServiceProvider
        {
            Name = name,
            Type = type,
            Description = "Description",
            CoverPhoto = new Uri("http://example.com"),
            Thumbnail = new Uri("http://example.com"),
            Address = "Address",
            Location = new LocationDTO(10, 10),
            Ratings = ratings,
        };

        await App.Command.RunSuccessAsync(createServiceProvider);

        var serviceProvider = await App.Query.GetAsync(new AllServiceProviders { PageSize = 100 }, cancellationToken);
        return serviceProvider.Items.Should().ContainSingle(e => e.Name == name).Which.Id;
    }
}
