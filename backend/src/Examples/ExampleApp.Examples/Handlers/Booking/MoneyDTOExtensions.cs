using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Domain.Booking;

namespace ExampleApp.Examples.Handlers.Booking;

public static class MoneyDTOExtensions
{
    public static MoneyDTO ToDTO(this Money m) => new((int)(m.Value * 100), m.Currency);

    public static Money ToDomain(this MoneyDTO m) => new(m.Value / 100m, m.Currency);
}
