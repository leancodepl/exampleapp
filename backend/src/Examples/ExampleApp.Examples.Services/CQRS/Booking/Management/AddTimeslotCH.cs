using ExampleApp.Examples.Contracts.Booking.Management;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Services.DataAccess.Queries;
using ExampleApp.Examples.Services.DataAccess.Repositories;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Services.CQRS.Booking.Management;

public class AddTimeslotCV : AbstractValidator<AddTimeslot>
{
    public AddTimeslotCV(ICalendarDayByDate calendarDays)
    {
        this.RuleForId(cmd => cmd.ServiceProviderId)
            .IsValid<ServiceProviderId>(AddTimeslot.ErrorCodes.ServiceProviderIdIsInvalid)
            .Exists<ServiceProvider>(AddTimeslot.ErrorCodes.ServiceProviderDoesNotExist)
            .DependentRules(() =>
            {
                RuleFor(cmd => cmd)
                    .CustomAsync(
                        async (cmd, ctx, ct) =>
                        {
                            var spId = ServiceProviderId.Parse(cmd.ServiceProviderId);
                            var day = await calendarDays.FindAsync(spId, cmd.Date, ct);

                            if (day is not null && !day.CanAddTimeslotAt(cmd.StartTime, cmd.EndTime))
                            {
                                ctx.AddValidationError(
                                    "The timeslot overlaps with existing timeslot.",
                                    AddTimeslot.ErrorCodes.TimeslotOverlapsWithExisting
                                );
                            }
                        }
                    );
            });

        RuleFor(cmd => cmd.EndTime)
            .GreaterThan(cmd => cmd.StartTime)
            .WithCode(AddTimeslot.ErrorCodes.EndTimeMustBeAfterStartTime);

        RuleFor(cmd => cmd.Price)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithCode(AddTimeslot.ErrorCodes.PriceIsNull)
            .Must(e => Money.IsValidCurrency(e.Currency))
            .WithCode(AddTimeslot.ErrorCodes.PriceCurrencyIsInvalid)
            .WithMessage("The currency is unsupported.");
    }
}

public class AddTimeslotCH(IRepository<CalendarDay, CalendarDayId> calendarDays, ICalendarDayByDate calendarByDate)
    : ICommandHandler<AddTimeslot>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<AddTimeslotCH>();

    public async Task ExecuteAsync(HttpContext context, AddTimeslot command)
    {
        var spId = ServiceProviderId.Parse(command.ServiceProviderId);
        var day = await calendarByDate.FindAsync(spId, command.Date, context.RequestAborted);
        if (day is null)
        {
            day = CalendarDay.Create(spId, command.Date);
            calendarDays.Add(day);
        }
        else
        {
            calendarDays.Update(day);
        }

        day.AddTimeslot(
            command.StartTime,
            command.EndTime,
            new((decimal)command.Price.Value / 100m, command.Price.Currency)
        );

        logger.Information("New timeslot added to provider {ServiceProviderId}", spId);
    }
}
