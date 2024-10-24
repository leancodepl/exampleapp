using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Booking.Events;
using LeanCode.DomainModels.DataAccess;
using MassTransit;

namespace ExampleApp.Examples.Handlers.Booking.Reservations;

public class ReserveTimeslotAfterReservationCreationEH(IRepository<CalendarDay, CalendarDayId> calendarDays)
    : IConsumer<ReservationCreated>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<ReserveTimeslotAfterReservationCreationEH>();

    public async Task Consume(ConsumeContext<ReservationCreated> context)
    {
        var message = context.Message;

        var calendarDay = await calendarDays.FindAndEnsureExistsAsync(message.CalendarDayId, context.CancellationToken);

        calendarDay.ReserveTimeslot(message.TimeslotId, message.ReservationId);

        calendarDays.Update(calendarDay);

        logger.Information(
            "Tried reserving timeslot {TimeslotId} for reservation {ReservationId}",
            message.TimeslotId,
            message.ReservationId
        );
    }
}
