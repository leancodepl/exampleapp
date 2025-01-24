using ExampleApp.Examples.Contracts.Booking.Reservations;
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

public class CancelReservationCVTests
{
    private readonly FakeReservationsRepository reservations = new();
    private readonly CancelReservationCV validator;

    public CancelReservationCVTests()
    {
        validator = new(reservations);
    }

    [Fact]
    public async Task Checks_if_TimeslotId_is_valid()
    {
        var invalidId = await ValidateAsync(new() { ReservationId = "invalid id" });

        invalidId
            .ShouldHaveValidationErrorFor(x => x.ReservationId)
            .Should()
            .HaveErrorCode(CancelReservation.ErrorCodes.ReservationIdIsInvalid);
    }

    [Fact]
    public async Task Checks_if_Reservation_exists()
    {
        var invalidReservation = await ValidateAsync(new() { ReservationId = ReservationId.New() });

        invalidReservation
            .ShouldHaveValidationErrorFor(x => x.ReservationId)
            .Should()
            .HaveErrorCode(CancelReservation.ErrorCodes.ReservationDoesNotExist);
    }

    [Fact]
    public async Task Checks_if_Reservation_can_be_cancelled()
    {
        var rId = AddReservation();

        var invalidReservation = await ValidateAsync(new() { ReservationId = rId });

        invalidReservation
            .ShouldHaveValidationErrorFor(x => x.ReservationId)
            .Should()
            .HaveErrorCode(CancelReservation.ErrorCodes.ReservationCannotBeCancelled);
    }

    [Fact]
    public async Task If_all_is_good_No_errors_are_returned()
    {
        var rId = AddReservation(r => r.Confirm());

        var invalidReservation = await ValidateAsync(new() { ReservationId = rId });

        invalidReservation.ShouldNotHaveAnyValidationErrors();
    }

    private ReservationId AddReservation(Action<Reservation>? postProcess = null)
    {
        var reservation = Reservation.Create(CalendarDayId.New(), TimeslotId.New(), CustomerId.New());
        postProcess?.Invoke(reservation);
        reservations.Add(reservation);
        return reservation.Id;
    }

    private async Task<TestValidationResult<CancelReservation>> ValidateAsync(CancelReservation command)
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices = new ServiceCollection()
                .AddSingleton<IRepository<Reservation, ReservationId>>(reservations)
                .BuildServiceProvider(),
        };
        var context = new ValidationContext<CancelReservation>(command)
        {
            RootContextData = { [ValidationContextExtensions.HttpContextKey] = httpContext },
        };
        return await validator.TestValidateAsync(context);
    }
}
