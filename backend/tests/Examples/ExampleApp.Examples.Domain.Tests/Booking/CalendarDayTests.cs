using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Booking.Events;
using FluentAssertions;
using LeanCode.UnitTests.TestHelpers;
using Xunit;

namespace ExampleApp.Examples.Domain.Tests.Booking;

public class CalendarDayTests
{
    public CalendarDayTests()
    {
        EventsInterceptor.Configure();
    }

    [Fact]
    public void Adds_new_timeslot()
    {
        var testDay = TestDay();

        var startTime = new TimeOnly(11, 0);
        var endTime = new TimeOnly(12, 0);
        var price = new Money(10m, "PLN");

        testDay.AddTimeslot(startTime, endTime, price);

        testDay
            .Timeslots.Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(
                new
                {
                    testDay.Date,
                    StartTime = startTime,
                    EndTime = endTime,
                    Price = price,
                }
            );
    }

    [Fact]
    public void Adds_multiple_nonoverlaping_timeslots()
    {
        var testDay = TestDay();

        testDay.AddTimeslot(new(9, 0), new(10, 0), new(10m, "PLN"));
        testDay.AddTimeslot(new(10, 30), new(11, 30), new(10m, "PLN"));
        testDay.AddTimeslot(new(11, 45), new(12, 45), new(10m, "PLN"));

        testDay.Timeslots.Should().HaveCount(3);
    }

    [Fact]
    public void Throws_an_exception_when_adding_overlapping_timeslot()
    {
        var testDay = TestDay();

        testDay.AddTimeslot(new(9, 0), new(10, 0), new(10m, "PLN"));

        var act = () => testDay.AddTimeslot(new(9, 30), new(10, 30), new(10m, "PLN"));

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Raises_a_TimeslotReserved_event_when_reserving_an_existing_timeslot()
    {
        var @event = EventsInterceptor.Single<TimeslotReserved>();

        var testDay = TestDay();
        testDay.AddTimeslot(new(11, 0), new(12, 0), new(10m, "PLN"));
        var timeslotId = testDay.Timeslots.First().Id;

        testDay.ReserveTimeslot(timeslotId);

        @event.Raised.Should().BeTrue();
    }

    [Fact]
    public void Raises_a_TimeslotUnavailable_event_when_reserving_a_non_existing_timeslot()
    {
        var @event = EventsInterceptor.Single<TimeslotUnavailable>();

        var testDay = TestDay();
        var nonExistingTimeslotId = TimeslotId.New();

        testDay.ReserveTimeslot(nonExistingTimeslotId);

        @event.Raised.Should().BeTrue();
    }

    [Theory]
    [InlineData("09:00", "10:00", false)]
    [InlineData("08:30", "09:30", false)]
    [InlineData("10:30", "11:30", true)]
    [InlineData("10:00", "11:00", true)]
    public void CanAddTimeslotAt_tests(string startTimeStr, string endTimeStr, bool expectedResult)
    {
        var testDay = TestDay();

        testDay.AddTimeslot(new(9, 0), new(10, 0), new(10m, "PLN"));

        var startTime = TimeOnly.Parse(startTimeStr);
        var endTime = TimeOnly.Parse(endTimeStr);

        testDay.CanAddTimeslotAt(startTime, endTime).Should().Be(expectedResult);
    }

    private CalendarDay TestDay()
    {
        return CalendarDay.Create(new ServiceProviderId(), new DateOnly(2024, 10, 2));
    }
}