using System.Security.Claims;
using ExampleApp.Examples.Contracts.Booking.Reservations.Authorization;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Handlers.Booking.Reservations.Authorization;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;
using CustomerId = ExampleApp.Examples.Domain.CustomerId;

namespace ExampleApp.Examples.Tests.Handlers.Booking;

public class WhenOwnsReservationAuthorizerTests
{
    private readonly FakeReservationsRepository reservations = new();
    private readonly WhenOwnsReservationAuthorizer authorizer;

    public WhenOwnsReservationAuthorizerTests()
    {
        authorizer = new(reservations);
    }

    [Fact]
    public async Task Authorizes_when_reservation_id_is_malformed()
    {
        var result = await authorizer.CheckIfAuthorizedAsync(
            Context(CustomerId.New()),
            new Payload("invalid id"),
            null
        );

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Authorizes_when_reservation_does_not_exist()
    {
        var result = await CheckAsync(CustomerId.New(), ReservationId.New());

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Authorizes_when_reservation_exists_and_user_matches()
    {
        var reservation = Reservation.Create(CalendarDayId.New(), TimeslotId.New(), CustomerId.New());

        var result = await CheckAsync(reservation.CustomerId, reservation.Id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Does_not_authorize_when_reservation_exists_and_user_does_not_match()
    {
        var reservation = Reservation.Create(CalendarDayId.New(), TimeslotId.New(), CustomerId.New());
        reservations.Add(reservation);

        var result = await CheckAsync(CustomerId.New(), reservation.Id);

        result.Should().BeFalse();
    }

    private Task<bool> CheckAsync(CustomerId userId, ReservationId reservationId)
    {
        return authorizer.CheckIfAuthorizedAsync(Context(userId), new Payload(reservationId), null);
    }

    private HttpContext Context(CustomerId userId) =>
        new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity([new("sub", userId.ToString())])) };

    private record Payload(string ReservationId) : WhenOwnsReservationAttribute.IReservationRelated
    {
        public Payload(ReservationId id)
            : this(id.ToString()) { }
    }
}

internal class FakeReservationsRepository : FakeRepositoryBase<Reservation, ReservationId> { }
