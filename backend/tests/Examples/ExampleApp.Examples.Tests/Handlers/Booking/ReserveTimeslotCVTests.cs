using ExampleApp.Examples.Contracts.Booking.Reservations;
using ExampleApp.Examples.DataAccess.Queries;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Handlers.Booking.Reservations;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExampleApp.Examples.Tests.Handlers.Booking;

public class ReserveTimeslotCVTests
{
    private readonly FakeCalendarDaysRepository calendarDays;
    private readonly ReserveTimeslotCV validator;

    public ReserveTimeslotCVTests()
    {
        calendarDays = new();
        validator = new(calendarDays);
    }

    [Fact]
    public async Task Checks_if_TimeslotId_is_valid()
    {
        var invalidTimeslot = await ValidateAsync(new() { TimeslotId = "invalid id" });
        var validTimeslot = await ValidateAsync(new() { TimeslotId = TimeslotId.New() });

        invalidTimeslot
            .ShouldHaveValidationErrorFor(x => x.TimeslotId)
            .Should()
            .HaveErrorCode(ReserveTimeslot.ErrorCodes.TimeslotIdInvalid);

        validTimeslot.ShouldNotHaveValidationErrorFor(x => x.TimeslotId);
    }

    [Fact]
    public async Task Checks_if_CalendarDay_exists()
    {
        var cdId = AddCalendarDay();

        var invalidDay = await ValidateAsync(new() { CalendarDayId = CalendarDayId.New() });
        var validDay = await ValidateAsync(new() { CalendarDayId = cdId });

        invalidDay
            .ShouldHaveValidationErrorFor(x => x.CalendarDayId)
            .Should()
            .HaveErrorCode(ReserveTimeslot.ErrorCodes.CalendarDayDoesNotExist);

        validDay.ShouldNotHaveValidationErrorFor(x => x.CalendarDayId);
    }

    [Fact]
    public async Task Checks_if_the_timeslot_can_be_reserved_NotExist_case()
    {
        var cdId = AddCalendarDay();

        var invalidDay = await ValidateAsync(new() { CalendarDayId = cdId, TimeslotId = TimeslotId.New() });

        invalidDay
            .ShouldHaveValidationErrorFor(x => x.CalendarDayId)
            .Should()
            .HaveErrorCode(ReserveTimeslot.ErrorCodes.TimeslotCannotBeReserved);
    }

    [Fact]
    public async Task Checks_if_the_timeslot_can_be_reserved_Valid_case()
    {
        var (cdId, tId) = AddCalendarDayWithTimeslot();

        var validDay = await ValidateAsync(new() { CalendarDayId = cdId, TimeslotId = tId });

        validDay.ShouldNotHaveValidationErrorFor(x => x.CalendarDayId);
    }

    private async Task<TestValidationResult<ReserveTimeslot>> ValidateAsync(ReserveTimeslot command)
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices = new ServiceCollection()
                .AddSingleton<IRepository<CalendarDay, CalendarDayId>>(calendarDays)
                .AddSingleton<ICalendarDayByDate>(calendarDays)
                .BuildServiceProvider(),
        };
        var context = new ValidationContext<ReserveTimeslot>(command)
        {
            RootContextData = { [ValidationContextExtensions.HttpContextKey] = httpContext },
        };
        return await validator.TestValidateAsync(context);
    }

    private CalendarDayId AddCalendarDay()
    {
        var day = CalendarDay.Create(ServiceProviderId.New(), new DateOnly(2024, 10, 7));
        calendarDays.Add(day);
        return day.Id;
    }

    private (CalendarDayId, TimeslotId) AddCalendarDayWithTimeslot()
    {
        var day = CalendarDay.Create(ServiceProviderId.New(), new DateOnly(2024, 10, 7));
        day.AddTimeslot(new(10, 0), new(11, 0), new(10m, "PLN"));
        calendarDays.Add(day);
        return (day.Id, day.Timeslots[0].Id);
    }
}
