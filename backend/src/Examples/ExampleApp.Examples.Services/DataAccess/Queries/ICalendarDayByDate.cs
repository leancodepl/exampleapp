using ExampleApp.Examples.Domain.Booking;

namespace ExampleApp.Examples.Services.DataAccess.Queries;

public interface ICalendarDayByDate
{
    Task<CalendarDay?> FindAsync(ServiceProviderId id, DateOnly date, CancellationToken cancellationToken = default);
}
