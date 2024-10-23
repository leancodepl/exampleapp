using System.Text.Json.Serialization;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;

namespace ExampleApp.Examples.Domain.Booking.Events;

public class ReservationCreated : IDomainEvent
{
    public Guid Id { get; }
    public DateTime DateOccurred { get; }

    public ReservationId ReservationId { get; }
    public CalendarDayId CalendarDayId { get; }
    public TimeslotId TimeslotId { get; }
    public Domain.CustomerId CustomerId { get; }

    public ReservationCreated(Reservation reservation)
    {
        Id = Guid.NewGuid();
        DateOccurred = Time.UtcNow;

        ReservationId = reservation.Id;
        CalendarDayId = reservation.CalendarDayId;
        TimeslotId = reservation.TimeslotId;
        CustomerId = reservation.CustomerId;
    }

    [JsonConstructor]
    public ReservationCreated(
        Guid id,
        DateTime dateOccurred,
        ReservationId reservationId,
        CalendarDayId calendarDayId,
        TimeslotId timeslotId,
        Domain.CustomerId customerId
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
