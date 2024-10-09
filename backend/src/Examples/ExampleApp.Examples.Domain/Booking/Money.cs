using System.Collections.Frozen;
using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples.Domain.Booking;

public record Money : ValueObject
{
    private static readonly FrozenSet<string> SupportedCurrencies = new[] { "PLN", "USD" }.ToFrozenSet(
        StringComparer.InvariantCultureIgnoreCase
    );

    public decimal Value { get; init; }
    public string Currency { get; init; }

    public Money(decimal value, string currency)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        if (!IsValidCurrency(currency))
        {
            throw new ArgumentException($"Currency {currency} is not supported.", nameof(currency));
        }

        Value = value;
        Currency = currency.ToUpperInvariant();
    }

    public static bool IsValidCurrency(string currency) => SupportedCurrencies.Contains(currency);
}
