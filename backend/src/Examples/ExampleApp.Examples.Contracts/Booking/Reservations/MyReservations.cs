using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.Reservations;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
public class MyReservations : PaginatedQuery<MyReservationDTO> { }
