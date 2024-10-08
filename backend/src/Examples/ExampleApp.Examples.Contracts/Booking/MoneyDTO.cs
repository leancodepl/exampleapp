namespace ExampleApp.Examples.Contracts.Booking;

/// <summary>
/// The DTO representing a monetary value, e.g. amount with a currency.
/// </summary>
/// <param name="Value">The amount of money, in the smallest currency unit (e.g. grosz, cent).</param>
/// <param name="Currency">The (three letter) currency name, e.g. PLN, USD.</param>
public record MoneyDTO(int Value, string Currency)
{
    public class ErrorCodes
    {
        public const int ValueCannotBeNegative = 1;
        public const int CurrencyIsInvalid = 2;
    }
}
