using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.Reservations;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
public class MyReservationById : IQuery<MyReservationDTO?>
{
    public string Id { get; set; }
}
