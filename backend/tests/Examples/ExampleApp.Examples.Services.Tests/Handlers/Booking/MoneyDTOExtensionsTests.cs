using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Services.Handlers.Booking;
using FluentAssertions;
using Xunit;

namespace ExampleApp.Examples.Services.Tests.Handlers.Booking;

public class MoneyDTOExtensionsTests
{
    [Fact]
    public void ToDTO_and_ToDomain_are_inverses()
    {
        var money = new Money(1.0m, "USD");

        money.ToDTO().ToDomain().Should().BeEquivalentTo(money);
    }

    [Fact]
    public void ToDTO_preserves_currency()
    {
        var money = new Money(1.0m, "USD");

        var dto = money.ToDTO();

        dto.Currency.Should().Be(money.Currency);
    }

    [Fact]
    public void ToDomain_preserves_currency()
    {
        var dto = new MoneyDTO(1, "USD");

        var money = dto.ToDomain();

        money.Currency.Should().Be(dto.Currency);
    }

    [Fact]
    public void ToDTO_converts_value_to_smallest_unit()
    {
        var money = new Money(1.23m, "USD");

        var dto = money.ToDTO();

        dto.Value.Should().Be(123);
    }

    [Fact]
    public void ToDomain_converts_value_to_proper_units()
    {
        var dto = new MoneyDTO(123, "USD");

        var money = dto.ToDomain();

        money.Value.Should().Be(1.23m);
    }
}
