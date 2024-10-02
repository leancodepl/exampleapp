using ExampleApp.Examples.Contracts.Booking.Management;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Services.DataAccess.Repositories;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using LeanCode.TimeProvider;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Services.CQRS.Booking.Management;

public class AddTimeslotCV : AbstractValidator<AddTimeslot>
{
    public AddTimeslotCV(ServiceProvidersRepository serviceProviders)
    {
        this.RuleForId(cmd => cmd.ServiceProviderId)
            .IsValid<ServiceProviderId>(AddTimeslot.ErrorCodes.ServiceProviderIdIsInvalid)
            .Exists<ServiceProvider>(AddTimeslot.ErrorCodes.ServiceProviderIdIsInvalid);

        RuleFor(cmd => cmd.Date)
            .GreaterThan(DateOnly.FromDateTime(Time.Now))
            .WithCode(AddTimeslot.ErrorCodes.CannotDefineSlotsInThePast);

        RuleFor(cmd => cmd.EndTime)
            .GreaterThan(cmd => cmd.StartTime)
            .WithCode(AddTimeslot.ErrorCodes.EndTimeMustBeAfterStartTime);

        RuleFor(cmd => cmd.Price)
            .NotNull()
            .WithCode(AddTimeslot.ErrorCodes.PriceIsNull)
            .Must(e => Money.IsValidCurrency(e.Currency))
            .WithCode(AddTimeslot.ErrorCodes.PriceCurrencyIsInvalid)
            .WithMessage("The currency is unsupported.");

        RuleFor(cmd => cmd)
            .CustomAsync(
                async (cmd, ctx, ct) =>
                {
                    var spId = ServiceProviderId.Parse(cmd.ServiceProviderId);
                    var sp = await serviceProviders.FindAndEnsureExistsAsync(spId, ct);

                    if (!sp.CanAddTimeslotAt(cmd.Date, cmd.StartTime, cmd.EndTime))
                    {
                        ctx.AddValidationError(
                            "The timeslot overlaps with existing timeslot.",
                            AddTimeslot.ErrorCodes.TimeslotOverlapsWithExisting
                        );
                    }
                }
            );
    }
}

public class AddTimeslotCH(IRepository<ServiceProvider, ServiceProviderId> serviceProviders)
    : ICommandHandler<AddTimeslot>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<AddTimeslotCH>();

    public async Task ExecuteAsync(HttpContext context, AddTimeslot command)
    {
        var spId = ServiceProviderId.Parse(command.ServiceProviderId);
        var sp = await serviceProviders.FindAndEnsureExistsAsync(spId, context.RequestAborted);
        sp.AddTimeslot(
            command.Date,
            command.StartTime,
            command.EndTime,
            new(command.Price.Value, command.Price.Currency)
        );

        logger.Information("New timeslot added to provider {ServiceProviderId}", spId);
    }
}
