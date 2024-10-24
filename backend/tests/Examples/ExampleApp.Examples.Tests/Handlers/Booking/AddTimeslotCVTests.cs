using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Contracts.Booking.Management;
using ExampleApp.Examples.DataAccess.Queries;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Handlers.Booking.Management;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ServiceProvider = ExampleApp.Examples.Domain.Booking.ServiceProvider;

namespace ExampleApp.Examples.Tests.Handlers.Booking;

public class AddTimeslotCVTests
{
    private readonly FakeCalendarDaysRepository calendarDays;
    private readonly FakeServiceProvidersRepository serviceProviders;
    private readonly AddTimeslotCV validator;

    public AddTimeslotCVTests()
    {
        calendarDays = new();
        serviceProviders = new();
        validator = new AddTimeslotCV(calendarDays);
    }

    [Fact]
    public async Task Check_if_StartTime_is_before_EndTime()
    {
        var result = await ValidateAsync(new() { StartTime = new(12, 0), EndTime = new(11, 0) });

        result
            .ShouldHaveValidationErrorFor(x => x.EndTime)
            .Should()
            .ContainSingle()
            .Which.CustomState.Should()
            .BeOfType<FluentValidatorErrorState>()
            .Which.ErrorCode.Should()
            .Be(AddTimeslot.ErrorCodes.EndTimeMustBeAfterStartTime);
    }

    [Fact]
    public async Task Validates_Price()
    {
        var result = await ValidateAsync(new());

        result
            .ShouldHaveValidationErrorFor(x => x.Price)
            .Should()
            .ContainSingle()
            .Which.CustomState.Should()
            .BeOfType<FluentValidatorErrorState>()
            .Which.ErrorCode.Should()
            .Be(AddTimeslot.ErrorCodes.PriceIsNull);
    }

    [Fact]
    public async Task Validates_Currency()
    {
        var invalidCurrency = await ValidateAsync(new() { Price = new(100, "abc") });
        var validCurrency = await ValidateAsync(new() { Price = new(100, "PLN") });

        invalidCurrency
            .ShouldHaveValidationErrorFor(x => x.Price.Currency)
            .Should()
            .HaveErrorCode(MoneyDTO.ErrorCodes.CurrencyIsInvalid);

        validCurrency.ShouldNotHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public async Task Checks_if_ServiceProvider_exists()
    {
        var spId = AddServiceProvider();

        var invalidProvider = await ValidateAsync(new() { ServiceProviderId = ServiceProviderId.New() });
        var validProvider = await ValidateAsync(new() { ServiceProviderId = spId });

        invalidProvider
            .ShouldHaveValidationErrorFor(x => x.ServiceProviderId)
            .Should()
            .HaveErrorCode(AddTimeslot.ErrorCodes.ServiceProviderDoesNotExist);

        validProvider.ShouldNotHaveValidationErrorFor(x => x.ServiceProviderId);
    }

    [Fact]
    public async Task Checks_if_timeslot_does_not_overlap_with_other()
    {
        var spId = AddServiceProvider();
        AddDayWithTimeslot(new(2024, 10, 7), new(11, 0), new(12, 0), spId);

        var overlappingResult = await ValidateAsync(
            new()
            {
                ServiceProviderId = spId,
                Date = new(2024, 10, 7),
                StartTime = new(11, 0),
                EndTime = new(12, 0),
            }
        );
        var nonoverlappingResult = await ValidateAsync(
            new()
            {
                ServiceProviderId = spId,
                Date = new(2024, 10, 8),
                StartTime = new(11, 0),
                EndTime = new(12, 0),
            }
        );

        overlappingResult
            .ShouldHaveValidationErrorFor(x => x)
            .Should()
            .HaveErrorCode(AddTimeslot.ErrorCodes.TimeslotOverlapsWithExisting);
        nonoverlappingResult.ShouldNotHaveValidationErrorFor(x => x);
    }

    private async Task<TestValidationResult<AddTimeslot>> ValidateAsync(AddTimeslot command)
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices = new ServiceCollection()
                .AddSingleton<IRepository<CalendarDay, CalendarDayId>>(calendarDays)
                .AddSingleton<ICalendarDayByDate>(calendarDays)
                .AddSingleton<IRepository<ServiceProvider, ServiceProviderId>>(serviceProviders)
                .BuildServiceProvider(),
        };
        var context = new ValidationContext<AddTimeslot>(command)
        {
            RootContextData = { [ValidationContextExtensions.HttpContextKey] = httpContext },
        };
        return await validator.TestValidateAsync(context, default);
    }

    private ServiceProviderId AddServiceProvider()
    {
        var sp = ServiceProvider.Create(
            "Test",
            ServiceProviderType.Hairdresser,
            "Description",
            new Uri("http://example.com"),
            new Uri("http://example.com"),
            "Address",
            new(10, 10),
            5
        );
        serviceProviders.Add(sp);
        return sp.Id;
    }

    private void AddDayWithTimeslot(DateOnly date, TimeOnly start, TimeOnly end, ServiceProviderId spId)
    {
        var day = CalendarDay.Create(spId, date);
        calendarDays.Add(day);

        day.AddTimeslot(start, end, new(100, "PLN"));
    }
}

internal class FakeServiceProvidersRepository : FakeRepositoryBase<ServiceProvider, ServiceProviderId> { }
