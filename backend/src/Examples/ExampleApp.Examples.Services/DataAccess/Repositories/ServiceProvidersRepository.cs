using ExampleApp.Examples.Domain.Booking;
using LeanCode.DomainModels.EF;

namespace ExampleApp.Examples.Services.DataAccess.Repositories;

public class ServiceProvidersRepository(ExamplesDbContext dbContext)
    : CachingEFRepository<ServiceProvider, ServiceProviderId, ExamplesDbContext>(dbContext);
