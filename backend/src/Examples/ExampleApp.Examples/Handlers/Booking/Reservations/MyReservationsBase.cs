using ExampleApp.Examples.Contracts.Booking;
using ExampleApp.Examples.Contracts.Booking.Reservations;
using ExampleApp.Examples.DataAccess;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Handlers.Booking.Reservations;

public class MyReservationsBase(ExamplesDbContext dbContext)
{
    protected IQueryable<MyReservationDTO> MyReservations(HttpContext context)
    {
        var customerId = context.GetCustomerId();
        return dbContext
            .Reservations.Where(r => r.CustomerId == customerId)
            .Join(
                dbContext.Timeslots,
                r => r.TimeslotId,
                t => t.Id,
                (reservation, timeslot) => new { reservation, timeslot }
            )
            .Join(
                dbContext.ServiceProviders,
                c => c.timeslot.ServiceProviderId,
                sp => sp.Id,
                (c, sp) =>
                    new MyReservationDTO
                    {
                        Id = c.reservation.Id,
                        TimeslotId = c.timeslot.Id,
                        ServiceProviderId = sp.Id,
                        ServiceProviderName = sp.Name,
                        Type = (ServiceProviderTypeDTO)sp.Type,
                        Address = sp.Address,
                        Location = sp.Location.ToDTO(),
                        Date = c.timeslot.Date,
                        StartTime = c.timeslot.StartTime,
                        EndTime = c.timeslot.EndTime,
                        Price = c.timeslot.Price.ToDTO(),
                        Status = (ReservationStatusDTO)c.reservation.Status,
                    }
            );
    }
}
