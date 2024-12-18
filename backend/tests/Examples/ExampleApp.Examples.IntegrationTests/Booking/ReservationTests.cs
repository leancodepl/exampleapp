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
    [Fact]
    public async Task Creating_reservation()
    {
        var spId = await CreateServiceProviderAsync();
        var timeslot1 = await AddTimeslotAsync(spId, hour: 14);
        var timeslot2 = await AddTimeslotAsync(spId, hour: 15);

        await App.Command.RunSuccessAsync(
            new ReserveTimeslot { TimeslotId = timeslot1.Id, CalendarDayId = timeslot1.CalendarDayId }
        );

        var detailsByTimeslot = await App.Query.GetAsync(new MyReservationByTimeslotId { TimeslotId = timeslot1.Id });
        detailsByTimeslot.Should().BeEquivalentTo(new { TimeslotId = timeslot1.Id, ServiceProviderId = spId });

        var detailsById = await App.Query.GetAsync(new MyReservationById { ReservationId = detailsByTimeslot!.Id });
        detailsById.Should().BeEquivalentTo(detailsByTimeslot);

        await App.WaitForBusAsync();

        var detailsByIdAfterConfirmation = await App.Query.GetAsync(
            new MyReservationById { ReservationId = detailsByTimeslot!.Id }
        );
        detailsByIdAfterConfirmation.Should().BeEquivalentTo(new { Status = ReservationStatusDTO.Confirmed });

        await App.Command.RunSuccessAsync(
            new ReserveTimeslot { TimeslotId = timeslot2.Id, CalendarDayId = timeslot2.CalendarDayId }
        );

        await App.WaitForBusAsync();

        var reservations = await App.Query.GetAsync(new MyReservations { PageSize = 100 });
        reservations
            .Items.Should()
            .BeEquivalentTo(
                [new { TimeslotId = timeslot2.Id }, new { TimeslotId = timeslot1.Id }],
                opts => opts.WithStrictOrdering()
            );
    }

    [Fact]
    public async Task Non_confirmed_reservations_are_not_returned_on_the_list()
    {
        var spId = await CreateServiceProviderAsync();
        var timeslot = await AddTimeslotAsync(spId);

        await App.Command.RunSuccessAsync(
            new ReserveTimeslot { TimeslotId = timeslot.Id, CalendarDayId = timeslot.CalendarDayId }
        );

        // This assumes that the bus will not stabilize between the command and the query.
        // This might be a far-fetched assumption, thus if this breaks, consider removing it.
        var reservations = await App.Query.GetAsync(new MyReservations());
        reservations.Items.Should().BeEmpty();
    }

    private async Task<TimeslotDTO> AddTimeslotAsync(string spId, int hour = 14)
    {
        var date = new DateOnly(2024, 10, 23);
        var from = new TimeOnly(hour, 0);
        var addTimeslot = new AddTimeslot
        {
            ServiceProviderId = spId,
            Date = date,
            StartTime = from,
            EndTime = new(from.Hour + 1, 0),
            Price = new MoneyDTO((int)(10m * 100), "PLN"),
        };

        await App.Command.RunSuccessAsync(addTimeslot);

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
