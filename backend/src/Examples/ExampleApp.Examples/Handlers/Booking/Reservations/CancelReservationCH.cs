using ExampleApp.Examples.Contracts.Booking.Reservations;
using ExampleApp.Examples.Domain.Booking;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace ExampleApp.Examples.Handlers.Booking.Reservations;

public class CancelReservationCV : AbstractValidator<CancelReservation>
{
    public CancelReservationCV(IRepository<Reservation, ReservationId> reservations)
    {
        this.RuleForId(x => x.ReservationId)
            .IsValid<ReservationId>(CancelReservation.ErrorCodes.ReservationIdIsInvalid)
            .Exists<Reservation>(CancelReservation.ErrorCodes.ReservationDoesNotExist)
            .CustomAsync(
                async (id, ctx, ct) =>
                {
                    if (ReservationId.TryParse(id, out var rId))
                    {
                        var reservation = await reservations.FindAsync(rId, ct);
                        if (reservation is not null && !reservation.CanCancel)
                        {
                            ctx.AddValidationError(
                                "Reservation cannot be cancelled.",
                                CancelReservation.ErrorCodes.ReservationCannotBeCancelled
                            );
                        }
                    }
                }
            );
    }
}

public class CancelReservationCH(IRepository<Reservation, ReservationId> reservations)
    : ICommandHandler<CancelReservation>
{
    private readonly ILogger logger = Log.ForContext<CancelReservationCH>();

    public async Task ExecuteAsync(HttpContext context, CancelReservation command)
    {
        var reservationId = ReservationId.Parse(command.ReservationId);
        var reservation = await reservations.FindAndEnsureExistsAsync(reservationId, context.RequestAborted);

        reservation.Cancel();
        reservations.Update(reservation);

        logger.Information("Reservation {ReservationId} cancelled", reservationId);
    }
}
