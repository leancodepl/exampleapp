using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Domain.Booking;
using FluentValidation;
using LeanCode.CQRS.Validation.Fluent;

namespace ExampleApp.Examples.Services.CQRS.Booking;

public class MoneyDTOValidator : AbstractValidator<MoneyDTO>
{
    public MoneyDTOValidator()
    {
        RuleFor(x => x.Value).GreaterThanOrEqualTo(0).WithCode(MoneyDTO.ErrorCodes.ValueCannotBeNegative);
        RuleFor(x => x.Currency)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithCode(MoneyDTO.ErrorCodes.CurrencyIsInvalid)
            .Must(Money.IsValidCurrency)
            .WithCode(MoneyDTO.ErrorCodes.CurrencyIsInvalid)
            .WithMessage("The currency is unsupported.");
    }
}
