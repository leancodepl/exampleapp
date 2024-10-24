using ExampleApp.Examples.Contracts;
using ExampleApp.Examples.Contracts.Booking.Reservations;
using ExampleApp.Examples.DataAccess;
using LeanCode.CQRS.Execution;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Handlers.Booking.Reservations;

public class MyReservationsQH(ExamplesDbContext dbContext)
    : MyReservationsBase(dbContext),
        IQueryHandler<MyReservations, PaginatedResult<MyReservationDTO>>
{
    public Task<PaginatedResult<MyReservationDTO>> ExecuteAsync(HttpContext context, MyReservations query)
    {
        return MyReservations(context)
            .Where(r => r.Status != ReservationStatusDTO.Pending && r.Status != ReservationStatusDTO.Rejected)
            .OrderByDescending(r => r.Date)
            .ThenByDescending(r => r.StartTime)
            .ToPaginatedResultAsync(query);
    }
}
