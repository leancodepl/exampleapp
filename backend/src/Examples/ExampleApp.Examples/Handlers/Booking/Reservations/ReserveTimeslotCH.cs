using ExampleApp.Examples.Contracts.Booking.Reservations;
using ExampleApp.Examples.Domain.Booking;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace ExampleApp.Examples.Handlers.Booking.Reservations;

public class ReserveTimeslotCV : AbstractValidator<ReserveTimeslot>
{
    public ReserveTimeslotCV(IRepository<CalendarDay, CalendarDayId> calendarDays)
    {
        this.RuleForId(e => e.TimeslotId).IsValid<TimeslotId>(ReserveTimeslot.ErrorCodes.TimeslotIdInvalid);

        this.RuleForId(e => e.CalendarDayId)
            .IsValid<CalendarDayId>(ReserveTimeslot.ErrorCodes.CalendarDayIdInvalid)
            .Exists<CalendarDay>(ReserveTimeslot.ErrorCodes.CalendarDayDoesNotExist)
            .CustomAsync(
                async (id, ctx, ct) =>
                {
                    if (
                        CalendarDayId.TryParse(id, out var cdId)
                        && TimeslotId.TryParse(ctx.InstanceToValidate.TimeslotId, out var tId)
                    )
                    {
                        var cd = await calendarDays.FindAndEnsureExistsAsync(cdId, ct);
                        if (!cd.CanReserveTimeslot(tId))
                        {
                            ctx.AddValidationError(
                                "The timeslot cannot be reserved.",
                                ReserveTimeslot.ErrorCodes.TimeslotCannotBeReserved
                            );
                        }
                    }
                }
            );
    }
}

public class ReserveTimeslotCH(IRepository<Reservation, ReservationId> reservations) : ICommandHandler<ReserveTimeslot>
{
    // create logger variable
    private readonly ILogger logger = Serilog.Log.ForContext<ReserveTimeslotCH>();

    public Task ExecuteAsync(HttpContext context, ReserveTimeslot command)
    {
        var calendarDayId = CalendarDayId.Parse(command.CalendarDayId);
        var timeslotId = TimeslotId.Parse(command.TimeslotId);
        var customerId = context.GetCustomerId();

        var reservation = Reservation.Create(calendarDayId, timeslotId, customerId);
        reservations.Add(reservation);

        logger.Information(
            "Customer {CustomerId} created reservation {ReservationId} for timeslot {TimeslotId} in day {CalendarDayId}",
            customerId,
            reservation.Id,
            timeslotId,
            calendarDayId
        );

        return Task.CompletedTask;
    }
}
