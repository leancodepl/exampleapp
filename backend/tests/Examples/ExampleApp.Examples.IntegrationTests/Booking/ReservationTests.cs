using ExampleApp.Examples.Contracts.Booking.Reservations;
using ExampleApp.Examples.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace ExampleApp.Examples.IntegrationTests.Booking;

public class BookingTests : BookingTestsBase
{
    [Fact]
    public async Task Creating_reservation()
    {
        var spId = await CreateServiceProviderAsync(cancellationToken: TestContext.Current.CancellationToken);
        var timeslot1 = await AddTimeslotAsync(
            spId,
            hour: 14,
            cancellationToken: TestContext.Current.CancellationToken
        );
        var timeslot2 = await AddTimeslotAsync(
            spId,
            hour: 15,
            cancellationToken: TestContext.Current.CancellationToken
        );

        await App.Command.RunSuccessAsync(
            new ReserveTimeslot { TimeslotId = timeslot1.Id, CalendarDayId = timeslot1.CalendarDayId }
        );
        await App.WaitForBusAsync();

        var detailsByTimeslot = await App.Query.GetAsync(
            new MyReservationByTimeslotId { TimeslotId = timeslot1.Id },
            TestContext.Current.CancellationToken
        );
        detailsByTimeslot.Should().BeEquivalentTo(new { TimeslotId = timeslot1.Id, ServiceProviderId = spId });

        var detailsById = await App.Query.GetAsync(
            new MyReservationById { ReservationId = detailsByTimeslot!.Id },
            TestContext.Current.CancellationToken
        );
        detailsById.Should().BeEquivalentTo(detailsByTimeslot);

        await App.WaitForBusAsync();

        var detailsByIdAfterConfirmation = await App.Query.GetAsync(
            new MyReservationById { ReservationId = detailsByTimeslot.Id },
            TestContext.Current.CancellationToken
        );
        detailsByIdAfterConfirmation.Should().BeEquivalentTo(new { Status = ReservationStatusDTO.Confirmed });

        await App.Command.RunSuccessAsync(
            new ReserveTimeslot { TimeslotId = timeslot2.Id, CalendarDayId = timeslot2.CalendarDayId }
        );

        await App.WaitForBusAsync();

        var reservations = await App.Query.GetAsync(
            new MyReservations { PageSize = 100 },
            TestContext.Current.CancellationToken
        );
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
        var spId = await CreateServiceProviderAsync(cancellationToken: TestContext.Current.CancellationToken);
        var timeslot = await AddTimeslotAsync(spId, cancellationToken: TestContext.Current.CancellationToken);

        await App.Command.RunSuccessAsync(
            new ReserveTimeslot { TimeslotId = timeslot.Id, CalendarDayId = timeslot.CalendarDayId }
        );

        // This assumes that the bus will not stabilize between the command and the query.
        // This might be a far-fetched assumption, thus if this breaks, consider removing it.
        var reservations = await App.Query.GetAsync(new MyReservations(), TestContext.Current.CancellationToken);
        reservations.Items.Should().BeEmpty();
    }
}
