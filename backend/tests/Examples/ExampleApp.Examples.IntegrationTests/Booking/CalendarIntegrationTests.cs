using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Contracts.Booking.Management;
using ExampleApp.Examples.Contracts.Booking.ServiceProviders;
using ExampleApp.Examples.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace ExampleApp.Examples.IntegrationTests.Booking;

public class CalendarIntegrationTests : TestsBase<AuthenticatedExampleAppTestApp>
{
    private const string Currency = "PLN";

    [Fact]
    public async Task Adding_and_listing_timeslots()
    {
        var sp = await CreateServiceProviderAsync();

        // Ensure that details work even without any timeslots
        var details = await App.Query.GetAsync(
            new ServiceProviderDetails { ServiceProviderId = sp, CalendarDate = new(2024, 10, 6) }
        );
        details.Should().NotBeNull();

        // Test different combination of timeslot
        await AddTimeslotAsync(sp, new(2024, 10, 6), new(10, 0), new(11, 0), 10.5m);
        await AddTimeslotAsync(sp, new(2024, 10, 7), new(11, 0), new(12, 0), 11);
        await AddTimeslotAsync(sp, new(2024, 10, 7), new(12, 0), new(13, 0), 12);

        var timeslots1 = await ListTimeslotsAsync(sp, new(2024, 10, 6));
        timeslots1
            .Should()
            .BeEquivalentTo(
                new[]
                {
                    new
                    {
                        StartTime = new TimeOnly(10, 0),
                        EndTime = new TimeOnly(11, 0),
                        Price = new MoneyDTO(1050, Currency),
                        IsReserved = false,
                    },
                }
            );

        var timeslots2 = await ListTimeslotsAsync(sp, new(2024, 10, 7));
        timeslots2
            .Should()
            .BeEquivalentTo(
                new[]
                {
                    new
                    {
                        StartTime = new TimeOnly(11, 0),
                        EndTime = new TimeOnly(12, 0),
                        Price = new MoneyDTO(1100, Currency),
                        IsReserved = false,
                    },
                    new
                    {
                        StartTime = new TimeOnly(12, 0),
                        EndTime = new TimeOnly(13, 0),
                        Price = new MoneyDTO(1200, Currency),
                        IsReserved = false,
                    },
                }
            );
    }

    private async Task<List<TimeslotDTO>> ListTimeslotsAsync(string spId, DateOnly date)
    {
        var details = await App.Query.GetAsync(
            new ServiceProviderDetails { ServiceProviderId = spId, CalendarDate = date }
        );
        details.Should().NotBeNull();
        return details!.Timeslots;
    }

    private async Task AddTimeslotAsync(string spId, DateOnly date, TimeOnly from, TimeOnly to, decimal price)
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

    private async Task<string> CreateServiceProviderAsync()
    {
        var fakeName = Guid.NewGuid().ToString();
        var createServiceProvider = new CreateServiceProvider
        {
            Name = fakeName,
            Type = ServiceProviderTypeDTO.Hairdresser,
            Description = "Description",
            PromotionalBanner = new Uri("http://example.com"),
            ListItemPicture = new Uri("http://example.com"),
            Address = "Address",
            Location = new LocationDTO(10, 10),
        };

        await App.Command.RunSuccessAsync(createServiceProvider);
        var serviceProvider = await App.Query.GetAsync(new AllServiceProviders { PageSize = 100 });
        return serviceProvider.Items.Should().ContainSingle(e => e.Name == fakeName).Which.Id;
    }
}
