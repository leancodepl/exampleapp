using ExampleApp.Examples.Domain.Booking.Events;
using LeanCode.DomainModels.Ids;
using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples.Domain.Booking;

[TypedId(TypedIdFormat.PrefixedUlid)]
public readonly partial record struct CalendarDayId;

public class CalendarDay : IAggregateRoot<CalendarDayId>
{
    private readonly List<Timeslot> timeslots = new();

    public CalendarDayId Id { get; private init; }

    public DateOnly Date { get; private init; }
    public ServiceProviderId ServiceProviderId { get; private init; }

    public IReadOnlyList<Timeslot> Timeslots => timeslots;

    DateTime IOptimisticConcurrency.DateModified { get; set; }

    private CalendarDay() { }

    public static CalendarDay Create(ServiceProviderId serviceProviderId, DateOnly date)
    {
        return new CalendarDay
        {
            Id = CalendarDayId.New(),
            Date = date,
            ServiceProviderId = serviceProviderId,
        };
    }

    public bool CanAddTimeslotAt(TimeOnly startTime, TimeOnly endTime) =>
        !timeslots.Any(ts => ts.StartTime < endTime && ts.EndTime > startTime);

    public void AddTimeslot(TimeOnly startTime, TimeOnly endTime, Money price)
    {
        if (!CanAddTimeslotAt(startTime, endTime))
        {
            throw new InvalidOperationException("The new timeslot overlaps with an existing timeslot.");
        }

        var newTimeslot = Timeslot.Create(this, startTime, endTime, price);
        timeslots.Add(newTimeslot);
    }

    public void ReserveTimeslot(TimeslotId timeslotId, ReservationId reservationId)
    {
        var timeslot = timeslots.Find(t => t.Id == timeslotId);
        if (timeslot is { ReservedBy: null })
        {
            timeslot.Reserve(reservationId);
            DomainEvents.Raise(new TimeslotReserved(Id, timeslotId, reservationId));
        }
        else
        {
            DomainEvents.Raise(new TimeslotUnavailable(Id, timeslotId, reservationId));
        }
    }

    public bool CanReserveTimeslot(TimeslotId timeslotId)
    {
        return timeslots.Find(t => t.Id == timeslotId) is { ReservedBy: null };
    }
}
