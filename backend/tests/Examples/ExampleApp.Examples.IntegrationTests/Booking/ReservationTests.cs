using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Contracts.Booking.Management;
using ExampleApp.Examples.Contracts.Booking.Reservations;
using ExampleApp.Examples.Contracts.Booking.ServiceProviders;
using ExampleApp.Examples.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace ExampleApp.Examples.IntegrationTests.Booking;

public class ReservationTests : TestsBase<AuthenticatedExampleAppTestApp>
{
    private const string Currency = "PLN";

    private int currentTimeslotHour = 0;

    [Fact]
    public async Task Creating_reservation()
    {
        var spId = await CreateServiceProviderAsync();
        var timeslot = await AddTimeslotAsync(spId);

        await App.Command.RunSuccessAsync(
            new ReserveTimeslot { TimeslotId = timeslot.Id, CalendarDayId = timeslot.CalendarDayId }
        );

        var detailsByTimeslot = await App.Query.GetAsync(new MyReservationByTimeslotId { TimeslotId = timeslot.Id });
        detailsByTimeslot.Should().BeEquivalentTo(new { TimeslotId = timeslot.Id, ServiceProviderId = spId });

        var detailsById = await App.Query.GetAsync(new MyReservationById { Id = detailsByTimeslot!.Id });
        detailsById.Should().BeEquivalentTo(detailsByTimeslot);

        await App.WaitForBusAsync();

        var detailsByIdAfterConfirmation = await App.Query.GetAsync(
            new MyReservationById { Id = detailsByTimeslot!.Id }
        );
        detailsByIdAfterConfirmation.Should().BeEquivalentTo(new { Status = ReservationStatusDTO.Confirmed });
    }

    private async Task<TimeslotDTO> AddTimeslotAsync(string spId)
    {
        var date = new DateOnly(2024, 10, 23);
        var from = new TimeOnly(currentTimeslotHour, 0);
        var addTimeslot = new AddTimeslot
        {
            ServiceProviderId = spId,
            Date = date,
            StartTime = from,
            EndTime = new(currentTimeslotHour + 1, 0),
            Price = new MoneyDTO((int)(10m * 100), Currency),
        };

        await App.Command.RunSuccessAsync(addTimeslot);
        currentTimeslotHour++;

        var details = await App.Query.GetAsync(
            new ServiceProviderDetails { ServiceProviderId = spId, CalendarDate = date }
        );
        return details!.Timeslots.Should().ContainSingle(t => t.StartTime == from).Which;
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
