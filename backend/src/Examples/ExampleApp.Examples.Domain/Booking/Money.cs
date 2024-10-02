using System.Collections.Frozen;
using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples.Domain.Booking;

public record Money : ValueObject
{
    public static FrozenSet<string> SupportedCurrencies = new[] { "PLN", "USD" }.ToFrozenSet();

    public decimal Value { get; init; }
    public string Currency { get; init; }

    public Money(decimal value, string currency)
    {
        if (!IsValidCurrency(currency))
        {
            throw new ArgumentException($"Currency {currency} is not supported.");
        }

        Value = value;
        Currency = currency;
    }

    public static bool IsValidCurrency(string currency) => SupportedCurrencies.Contains(currency);
}
