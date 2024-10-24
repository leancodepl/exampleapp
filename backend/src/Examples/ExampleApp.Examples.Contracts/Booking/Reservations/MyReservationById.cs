using ExampleApp.Examples.Contracts.Booking.Reservations.Authorization;
using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.Reservations;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
[AuthorizeWhenOwnsReservation]
public class MyReservationById : IQuery<MyReservationDTO?>, AuthorizeWhenOwnsReservationAttribute.IReservationRelated
{
    public string ReservationId { get; set; }
}
