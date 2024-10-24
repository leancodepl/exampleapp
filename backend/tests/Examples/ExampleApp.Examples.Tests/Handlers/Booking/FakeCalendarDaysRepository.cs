using ExampleApp.Examples.DataAccess.Queries;
using ExampleApp.Examples.Domain.Booking;

namespace ExampleApp.Examples.Tests.Handlers.Booking;

internal class FakeCalendarDaysRepository : FakeRepositoryBase<CalendarDay, CalendarDayId>, ICalendarDayByDate
{
    public Task<CalendarDay?> FindAsync(
        ServiceProviderId id,
        DateOnly date,
        CancellationToken cancellationToken = default
    ) => Task.FromResult(Storage.Values.FirstOrDefault(d => d.ServiceProviderId == id && d.Date == date));
}
