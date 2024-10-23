using ExampleApp.Examples.Contracts.Booking.Reservations;
using ExampleApp.Examples.DataAccess;
using ExampleApp.Examples.Domain.Booking;
using LeanCode.CQRS.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Handlers.Booking.Reservations;

public class MyReservationByTimeslotIdQH(ExamplesDbContext dbContext)
    : MyReservationsBase(dbContext),
        IQueryHandler<MyReservationByTimeslotId, MyReservationDTO?>
{
    public Task<MyReservationDTO?> ExecuteAsync(HttpContext context, MyReservationByTimeslotId query)
    {
        if (!TimeslotId.TryParse(query.TimeslotId, out var tId))
        {
            return Task.FromResult<MyReservationDTO?>(null);
        }

        return MyReservations(context).SingleOrDefaultAsync(r => r.TimeslotId == tId);
    }
}
