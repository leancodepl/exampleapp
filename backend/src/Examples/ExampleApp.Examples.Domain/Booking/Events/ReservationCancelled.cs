using System.Text.Json.Serialization;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;

namespace ExampleApp.Examples.Domain.Booking.Events;

public class ReservationCancelled : IDomainEvent
{
    public Guid Id { get; }
    public DateTime DateOccurred { get; }

    public ReservationId ReservationId { get; }
    public CalendarDayId CalendarDayId { get; }
    public TimeslotId TimeslotId { get; }
    public CustomerId CustomerId { get; }

    public ReservationCancelled(Reservation reservation)
    {
        Id = Guid.NewGuid();
        DateOccurred = Time.UtcNow;

        ReservationId = reservation.Id;
        CalendarDayId = reservation.CalendarDayId;
        TimeslotId = reservation.TimeslotId;
        CustomerId = reservation.CustomerId;
    }

    [JsonConstructor]
    public ReservationCancelled(
        Guid id,
        DateTime dateOccurred,
        ReservationId reservationId,
        CalendarDayId calendarDayId,
        TimeslotId timeslotId,
        CustomerId customerId
    )
    {
        Id = id;
        DateOccurred = dateOccurred;
        ReservationId = reservationId;
        CalendarDayId = calendarDayId;
        TimeslotId = timeslotId;
        CustomerId = customerId;
    }
}
