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
                reservation => reservation.TimeslotId,
                timeslot => timeslot.Id,
                (reservation, timeslot) => new { reservation, timeslot }
            )
            .Join(
                dbContext.ServiceProviders,
                combined => combined.timeslot.ServiceProviderId,
                serviceProvider => serviceProvider.Id,
                (combined, serviceProvider) =>
                    new MyReservationDTO
                    {
                        Id = combined.reservation.Id,
                        TimeslotId = combined.timeslot.Id,
                        ServiceProviderId = serviceProvider.Id,
                        ServiceProviderName = serviceProvider.Name,
                        Type = (ServiceProviderTypeDTO)serviceProvider.Type,
                        Address = serviceProvider.Address,
                        Location = serviceProvider.Location.ToDTO(),
                        StartTime = combined.timeslot.StartTime,
                        EndTime = combined.timeslot.EndTime,
                        Price = combined.timeslot.Price.ToDTO(),
                        Status = (ReservationStatusDTO)combined.reservation.Status,
                    }
            );
    }
}
