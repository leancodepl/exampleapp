using ExampleApp.Examples.Domain.Booking;
using LeanCode.DomainModels.EF;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.DataAccess.Repositories;

public class ServiceProvidersRepository : EFRepository<ServiceProvider, ServiceProviderId, ExamplesDbContext>
{
    public ServiceProvidersRepository(ExamplesDbContext dbContext)
        : base(dbContext) { }

    public override Task<ServiceProvider?> FindAsync(
        ServiceProviderId id,
        CancellationToken cancellationToken = default
    )
    {
        return DbSet.AsTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
