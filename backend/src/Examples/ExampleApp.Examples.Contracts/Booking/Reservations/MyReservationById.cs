using ExampleApp.Examples.Contracts.Booking.Reservations.Authorization;
using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.Reservations;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
[WhenOwnsReservation]
public class MyReservationById : IQuery<MyReservationDTO?>, WhenOwnsReservationAttribute.IReservationRelated
{
    public string ReservationId { get; set; }
}
