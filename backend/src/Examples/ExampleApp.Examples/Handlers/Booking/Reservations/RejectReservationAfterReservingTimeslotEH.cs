using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Booking.Events;
using LeanCode.DomainModels.DataAccess;
using MassTransit;

namespace ExampleApp.Examples.Handlers.Booking.Reservations;

public class RejectReservationAfterReservingTimeslotEH(IRepository<Reservation, ReservationId> reservations)
    : IConsumer<TimeslotUnavailable>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<RejectReservationAfterReservingTimeslotEH>();

    public async Task Consume(ConsumeContext<TimeslotUnavailable> context)
    {
        var message = context.Message;

        var reservation = await reservations.FindAndEnsureExistsAsync(message.ReservationId, context.CancellationToken);

        reservation.Reject();

        reservations.Update(reservation);

        logger.Information("Reservation {ReservationId} rejected", message.ReservationId);
    }
}
