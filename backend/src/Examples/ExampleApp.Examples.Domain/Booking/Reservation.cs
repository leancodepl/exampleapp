using ExampleApp.Examples.Domain.Booking.Events;
using LeanCode.DomainModels.Ids;
using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples.Domain.Booking;

[TypedId(TypedIdFormat.PrefixedUlid)]
public readonly partial record struct ReservationId;

public class Reservation : IAggregateRoot<ReservationId>
{
    public ReservationId Id { get; private init; }

    public CalendarDayId CalendarDayId { get; private init; }
    public TimeslotId TimeslotId { get; private init; }
    public CustomerId CustomerId { get; private init; }
    public ReservationStatus Status { get; private set; }

    public DateTime DateCreated { get; private init; }

    DateTime IOptimisticConcurrency.DateModified { get; set; }

    private Reservation() { }

    public static Reservation Create(CalendarDayId calendarDayId, TimeslotId timeslotId, CustomerId customerId)
    {
        var r = new Reservation
        {
            Id = ReservationId.New(),
            CalendarDayId = calendarDayId,
            TimeslotId = timeslotId,
            CustomerId = customerId,
            DateCreated = DateTime.UtcNow,
            Status = ReservationStatus.Pending,
        };
        DomainEvents.Raise(new ReservationCreated(r));
        return r;
    }

    public void Confirm()
    {
        if (Status != ReservationStatus.Pending)
        {
            throw new InvalidOperationException("Cannot confirm reservation - it is not pending.");
        }

        Status = ReservationStatus.Confirmed;
    }

    public void Reject()
    {
        if (Status != ReservationStatus.Pending)
        {
            throw new InvalidOperationException("Cannot reject reservation - it is not pending.");
        }

        Status = ReservationStatus.Rejected;
    }

    public bool CanCancel => Status == ReservationStatus.Confirmed;

    public void Cancel()
    {
        if (!CanCancel)
        {
            throw new InvalidOperationException("Cannot cancel reservation - it is not confirmed yet.");
        }

        Status = ReservationStatus.Cancelled;
        DomainEvents.Raise(new ReservationCancelled(this));
    }
}

public enum ReservationStatus
{
    Pending = 0,
    Confirmed = 1,
    Rejected = 2,
    Paid = 3,
    Cancelled = 4,
    Completed = 5,
}
