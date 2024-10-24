using ExampleApp.Examples.Contracts.Booking.Reservations;
using ExampleApp.Examples.DataAccess;
using ExampleApp.Examples.Domain.Booking;
using LeanCode.CQRS.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Handlers.Booking.Reservations;

public class MyReservationByIdQH(ExamplesDbContext dbContext)
    : MyReservationsBase(dbContext),
        IQueryHandler<MyReservationById, MyReservationDTO?>
{
    public Task<MyReservationDTO?> ExecuteAsync(HttpContext context, MyReservationById query)
    {
        if (!ReservationId.TryParse(query.ReservationId, out var rId))
        {
            return Task.FromResult<MyReservationDTO?>(null);
        }

        return MyReservations(context).FirstOrDefaultAsync(r => r.Id == rId);
    }
}
