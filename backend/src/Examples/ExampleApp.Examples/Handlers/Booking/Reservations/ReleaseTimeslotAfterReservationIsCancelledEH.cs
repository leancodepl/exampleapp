using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Booking.Events;
using LeanCode.DomainModels.DataAccess;
using MassTransit;

namespace ExampleApp.Examples.Handlers.Booking.Reservations;

public class ReleaseTimeslotAfterReservationIsCancelledEH(IRepository<CalendarDay, CalendarDayId> calendarDays)
    : IConsumer<ReservationCancelled>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<ReleaseTimeslotAfterReservationIsCancelledEH>();

    public async Task Consume(ConsumeContext<ReservationCancelled> context)
    {
        var message = context.Message;

        var calendarDay = await calendarDays.FindAndEnsureExistsAsync(message.CalendarDayId, context.CancellationToken);

        calendarDay.ReleaseTimeslot(message.TimeslotId, message.ReservationId);

        calendarDays.Update(calendarDay);

        logger.Information(
            "Tried releasing timeslot {TimeslotId} after reservation {ReservationId} cancellation",
            message.TimeslotId,
            message.ReservationId
        );
    }
}
