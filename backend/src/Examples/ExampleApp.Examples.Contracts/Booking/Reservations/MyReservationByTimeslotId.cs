using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.Reservations;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
public class MyReservationByTimeslotId : IQuery<MyReservationDTO?>
{
    public string TimeslotId { get; set; }
}
