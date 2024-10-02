using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Booking.Events;
using FluentAssertions;
using LeanCode.UnitTests.TestHelpers;
using Xunit;

namespace ExampleApp.Examples.Domain.Tests.Booking;

public class ServiceProviderTests
{
    public ServiceProviderTests()
    {
        EventsInterceptor.Configure();
    }

    [Fact]
    public void Adds_new_timeslot()
    {
        var provider = TestProvider();

        var date = new DateOnly(2024, 10, 2);
        var startTime = new TimeOnly(11, 0);
        var endTime = new TimeOnly(12, 0);
        var price = new Money(10m, "PLN");

        provider.AddTimeslot(date, startTime, endTime, price);

        provider
            .Timeslots.Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(
                new
                {
                    Date = date,
                    StartTime = startTime,
                    EndTime = endTime,
                    Price = price,
                }
            );
    }

    [Fact]
    public void Adds_multiple_nonoverlaping_timeslots()
    {
        var provider = TestProvider();

        provider.AddTimeslot(new(2024, 10, 2), new(9, 0), new(10, 0), new(10m, "PLN"));
        provider.AddTimeslot(new(2024, 10, 2), new(10, 30), new(11, 30), new(10m, "PLN"));
        provider.AddTimeslot(new(2024, 10, 2), new(11, 45), new(12, 45), new(10m, "PLN"));

        provider.Timeslots.Should().HaveCount(3);
    }

    [Fact]
    public void Throws_an_exception_when_adding_overlapping_timeslot()
    {
        var provider = TestProvider();

        provider.AddTimeslot(new(2024, 10, 2), new(9, 0), new(10, 0), new(10m, "PLN"));

        var act = () => provider.AddTimeslot(new(2024, 10, 2), new(9, 30), new(10, 30), new(10m, "PLN"));

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Raises_a_TimeslotReserved_event_when_reserving_an_existing_timeslot()
    {
        var @event = EventsInterceptor.Single<TimeslotReserved>();

        var provider = TestProvider();
        provider.AddTimeslot(new(2024, 10, 2), new(11, 0), new(12, 0), new(10m, "PLN"));
        var timeslotId = provider.Timeslots.First().Id;

        provider.ReserveTimeslot(timeslotId);

        @event.Raised.Should().BeTrue();
    }

    [Fact]
    public void Raises_a_TimeslotUnavailable_event_when_reserving_a_non_existing_timeslot()
    {
        var @event = EventsInterceptor.Single<TimeslotUnavailable>();

        var provider = TestProvider();
        var nonExistingTimeslotId = TimeslotId.New();

        provider.ReserveTimeslot(nonExistingTimeslotId);

        @event.Raised.Should().BeTrue();
    }

    [Theory]
    [InlineData("2024-10-02", "09:00", "10:00", false)]
    [InlineData("2024-10-02", "08:30", "09:30", false)]
    [InlineData("2024-10-02", "10:30", "11:30", true)]
    [InlineData("2024-10-02", "10:00", "11:00", true)]
    public void CanAddTimeslotAt_tests(string dateStr, string startTimeStr, string endTimeStr, bool expectedResult)
    {
        var provider = TestProvider();

        provider.AddTimeslot(new(2024, 10, 2), new(9, 0), new(10, 0), new(10m, "PLN"));

        var date = DateOnly.Parse(dateStr);
        var startTime = TimeOnly.Parse(startTimeStr);
        var endTime = TimeOnly.Parse(endTimeStr);

        provider.CanAddTimeslotAt(date, startTime, endTime).Should().Be(expectedResult);
    }

    private ServiceProvider TestProvider()
    {
        return ServiceProvider.Create(
            "Example ServiceProvider",
            ServiceProviderType.Hairdresser,
            "This is a sample description of a service provider.",
            new Uri("https://example.com/promotionalbanner.jpg"),
            new Uri("https://example.com/listitempicture.jpg"),
            "1234 Example Street, Example City, EX 12345",
            new Location(12.345678, 98.765432)
        );
    }
}
