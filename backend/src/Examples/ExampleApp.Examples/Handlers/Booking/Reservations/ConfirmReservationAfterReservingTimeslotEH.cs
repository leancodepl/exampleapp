using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Booking.Events;
using LeanCode.DomainModels.DataAccess;
using MassTransit;

namespace ExampleApp.Examples.Handlers.Booking.Reservations;

public class ConfirmReservationAfterReservingTimeslotEH(IRepository<Reservation, ReservationId> reservations)
    : IConsumer<TimeslotReserved>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<ConfirmReservationAfterReservingTimeslotEH>();

    public async Task Consume(ConsumeContext<TimeslotReserved> context)
    {
        var message = context.Message;

        var reservation = await reservations.FindAndEnsureExistsAsync(message.ReservationId, context.CancellationToken);

        reservation.Confirm();

        reservations.Update(reservation);

        logger.Information("Reservation {ReservationId} confirmed", message.ReservationId);
    }
}
