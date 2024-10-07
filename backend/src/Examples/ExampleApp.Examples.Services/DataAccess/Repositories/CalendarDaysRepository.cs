using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Services.DataAccess.Queries;
using LeanCode.DomainModels.EF;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.DataAccess.Repositories;

public class CalendarDaysRepository(ExamplesDbContext dbContext)
    : CachingEFRepository<CalendarDay, CalendarDayId, ExamplesDbContext>(dbContext),
        ICalendarDayByDate
{
    protected override IQueryable<CalendarDay> BaseQuery() => DbContext.CalendarDays.Include(e => e.Timeslots);

    Task<CalendarDay?> ICalendarDayByDate.FindAsync(
        ServiceProviderId id,
        DateOnly date,
        CancellationToken cancellationToken
    )
    {
        return DbSet
            .AsTracking()
            .Include(e => e.Timeslots)
            .FirstOrDefaultAsync(e => e.ServiceProviderId == id && e.Date == date, cancellationToken);
    }
}
