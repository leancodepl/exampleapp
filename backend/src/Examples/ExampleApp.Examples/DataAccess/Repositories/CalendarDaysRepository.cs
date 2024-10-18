using ExampleApp.Examples.DataAccess.Queries;
using ExampleApp.Examples.Domain.Booking;
using LeanCode.DomainModels.EF;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.DataAccess.Repositories;

public class CalendarDaysRepository(ExamplesDbContext dbContext)
    : CachingEFRepository<CalendarDay, CalendarDayId, ExamplesDbContext>(dbContext),
        ICalendarDayByDate
{
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
