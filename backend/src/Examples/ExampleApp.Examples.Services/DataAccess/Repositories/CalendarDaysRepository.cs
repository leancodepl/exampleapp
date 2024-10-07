using ExampleApp.Examples.Domain.Booking;
using LeanCode.DomainModels.EF;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.DataAccess.Repositories;

public class CalendarDaysRepository(ExamplesDbContext dbContext)
    : CachingEFRepository<CalendarDay, CalendarDayId, ExamplesDbContext>(dbContext)
{
    protected override IQueryable<CalendarDay> BaseQuery() => DbContext.CalendarDays.Include(e => e.Timeslots);

    public Task<CalendarDay?> FindByDateAsync(
        ServiceProviderId id,
        DateOnly date,
        CancellationToken cancellationToken = default
    )
    {
        return DbSet
            .AsTracking()
            .Include(e => e.Timeslots)
            .FirstOrDefaultAsync(e => e.ServiceProviderId == id && e.Date == date, cancellationToken);
    }
}
