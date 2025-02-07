using ExampleApp.Examples.Contracts.Booking.Reservations;
using ExampleApp.Examples.Contracts.Booking.ServiceProviders;
using ExampleApp.Examples.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace ExampleApp.Examples.IntegrationTests.Booking;

public class BookingTestsCancellation : BookingTestsBase
{
    [Fact]
    public async Task Cancelling_reservation()
    {
        var spId = await CreateServiceProviderAsync(cancellationToken: TestContext.Current.CancellationToken);
        var timeslot1 = await AddTimeslotAsync(
            spId,
            hour: 14,
            cancellationToken: TestContext.Current.CancellationToken
        );
        var reservation = await ReserveAsync(timeslot1);

        reservation.Status.Should().Be(ReservationStatusDTO.Confirmed);

        await App.Command.RunSuccessAsync(new CancelReservation { ReservationId = reservation.Id });
        await App.WaitForBusAsync();

        var updatedReservation = await App.Query.GetAsync(
            new MyReservationById { ReservationId = reservation.Id },
            TestContext.Current.CancellationToken
        );
        updatedReservation.Should().NotBeNull().And.BeEquivalentTo(new { Status = ReservationStatusDTO.Cancelled });

        var timeslots = await ListTimeslotsAsync(spId, cancellationToken: TestContext.Current.CancellationToken);
        timeslots.Should().ContainSingle(e => e.StartTime.Hour == 14).Which.IsReserved.Should().BeFalse();
    }

    private async Task<MyReservationDTO> ReserveAsync(TimeslotDTO timeslot1)
    {
        await App.Command.RunSuccessAsync(
            new ReserveTimeslot { TimeslotId = timeslot1.Id, CalendarDayId = timeslot1.CalendarDayId }
        );
        await App.WaitForBusAsync();

        var reservation = await App.Query.GetAsync(new MyReservationByTimeslotId { TimeslotId = timeslot1.Id });
        reservation.Should().NotBeNull();
        return reservation!;
    }
}
