using System.Linq.Expressions;
using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Contracts.Booking.Reservations;
using ExampleApp.Examples.DataAccess;
using ExampleApp.Examples.Domain.Booking;
using LeanCode.QueryableExtensions;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Handlers.Booking.Reservations;

public class MyReservationsBase(ExamplesDbContext dbContext)
{
    protected IQueryable<MyReservationDTO> MyReservations(
        HttpContext context,
        Expression<Func<Reservation, bool>>? predicate = null
    )
    {
        var customerId = context.GetCustomerId();
        return dbContext
            .Reservations.Where(r => r.CustomerId == customerId)
            .ConditionalWhere(predicate!, predicate is not null)
            .Join(dbContext.Timeslots, r => r.TimeslotId, t => t.Id, (r, t) => new { Reservation = r, Timeslot = t })
            .Join(
                dbContext.ServiceProviders,
                c => c.Timeslot.ServiceProviderId,
                sp => sp.Id,
                (c, sp) =>
                    new MyReservationDTO
                    {
                        Id = c.Reservation.Id,
                        TimeslotId = c.Timeslot.Id,
                        ServiceProviderId = sp.Id,
                        ServiceProviderName = sp.Name,
                        Type = (ServiceProviderTypeDTO)sp.Type,
                        Address = sp.Address,
                        Location = sp.Location.ToDTO(),
                        Date = c.Timeslot.Date,
                        StartTime = c.Timeslot.StartTime,
                        EndTime = c.Timeslot.EndTime,
                        Price = c.Timeslot.Price.ToDTO(),
                        Status = (ReservationStatusDTO)c.Reservation.Status,
                    }
            );
    }
}
