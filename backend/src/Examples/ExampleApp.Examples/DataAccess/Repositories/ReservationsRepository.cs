using ExampleApp.Examples.Domain.Booking;
using LeanCode.DomainModels.EF;

namespace ExampleApp.Examples.DataAccess.Repositories;

public class ReservationsRepository(ExamplesDbContext dbContext)
    : CachingEFRepository<Reservation, ReservationId, ExamplesDbContext>(dbContext);
