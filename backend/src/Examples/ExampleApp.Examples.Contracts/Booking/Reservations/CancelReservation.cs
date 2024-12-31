using ExampleApp.Examples.Contracts.Booking.Reservations.Authorization;
using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.Reservations;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
[AuthorizeWhenOwnsReservation]
public class CancelReservation : ICommand, AuthorizeWhenOwnsReservationAttribute.IReservationRelated
{
    public string ReservationId { get; set; }

    public static class ErrorCodes
    {
        public const int ReservationIdIsInvalid = 1;
        public const int ReservationDoesNotExist = 2;
        public const int ReservationCannotBeCancelled = 3;
    }
}
