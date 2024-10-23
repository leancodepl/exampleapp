using LeanCode.DomainModels.Ids;
using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples.Domain.Booking;

[TypedId(TypedIdFormat.PrefixedUlid, CustomPrefix = "timeslot")]
public readonly partial record struct TimeslotId;

public class Timeslot : IEntity<TimeslotId>
{
    public TimeslotId Id { get; private init; }

    public ServiceProviderId ServiceProviderId { get; private init; }
    public CalendarDayId CalendarDayId { get; private init; }
    public CalendarDay CalendarDay { get; private init; }

    public DateOnly Date { get; private init; }
    public TimeOnly StartTime { get; private init; }
    public TimeOnly EndTime { get; private init; }
    public Money Price { get; private init; }

    public bool IsReserved { get; private set; }

    private Timeslot()
    {
        CalendarDay = null!;
        Price = null!;
    }

    internal static Timeslot Create(CalendarDay day, TimeOnly startTime, TimeOnly endTime, Money price)
    {
        if (startTime >= endTime)
        {
            throw new ArgumentException("Start time must be before end time.");
        }

        return new Timeslot
        {
            Id = TimeslotId.New(),
            ServiceProviderId = day.ServiceProviderId,
            CalendarDayId = day.Id,
            CalendarDay = day,
            Date = day.Date,
            StartTime = startTime,
            EndTime = endTime,
            Price = price,
        };
    }

    internal void Reserve()
    {
        if (IsReserved)
        {
            throw new InvalidOperationException("Timeslot is already reserved.");
        }

        IsReserved = true;
    }
}
