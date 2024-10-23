using ExampleApp.Examples.Contracts.Booking.Reservations.Authorization;
using ExampleApp.Examples.Domain.Booking;
using LeanCode.CQRS.Security;
using LeanCode.DomainModels.DataAccess;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Handlers.Booking.Reservations.Authorization;

public class WhenOwnsReservationAuthorizer(IRepository<Reservation, ReservationId> reservations)
    : HttpContextCustomAuthorizer<WhenOwnsReservationAttribute.IReservationRelated>,
        WhenOwnsReservationAttribute.IWhenOwnsReservation
{
    protected override async Task<bool> CheckIfAuthorizedAsync(
        HttpContext context,
        WhenOwnsReservationAttribute.IReservationRelated obj
    )
    {
        if (ReservationId.TryParse(obj.ReservationId, out var rId))
        {
            var reservation = await reservations.FindAsync(rId, context.RequestAborted);
            return reservation is null || reservation.CustomerId == context.GetCustomerId();
        }
        else
        {
            return true;
        }
    }
}
