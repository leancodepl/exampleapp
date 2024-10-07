using System.Text.Json.Serialization;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;

namespace ExampleApp.Examples.Domain.Booking.Events;

public class TimeslotReserved : IDomainEvent
{
    public Guid Id { get; }
    public DateTime DateOccurred { get; }

    public CalendarDayId CalendarDayId { get; }
    public TimeslotId TimeslotId { get; }

    public TimeslotReserved(CalendarDayId calendarDayId, TimeslotId timeslotId)
    {
        Id = Guid.NewGuid();
        DateOccurred = Time.UtcNow;

        CalendarDayId = calendarDayId;
        TimeslotId = timeslotId;
    }

    [JsonConstructor]
    public TimeslotReserved(Guid id, DateTime dateOccurred, CalendarDayId calendarDayId, TimeslotId timeslotId)
    {
        Id = id;
        DateOccurred = dateOccurred;
        CalendarDayId = calendarDayId;
        TimeslotId = timeslotId;
    }
}
