using System.Text.Json.Serialization;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;

namespace ExampleApp.Examples.Domain.Booking.Events;

public class ReservationCreated : IDomainEvent
{
    public Guid Id { get; }
    public DateTime DateOccurred { get; }

    public ReservationId ReservationId { get; }
    public CustomerId CustomerId { get; }

    public ReservationCreated(Reservation reservation)
    {
        Id = Guid.NewGuid();
        DateOccurred = Time.UtcNow;

        ReservationId = reservation.Id;
        CustomerId = reservation.CustomerId;
    }

    [JsonConstructor]
    public ReservationCreated(Guid id, DateTime dateOccurred, ReservationId reservationId, CustomerId customerId)
    {
        Id = id;
        DateOccurred = dateOccurred;
        ReservationId = reservationId;
        CustomerId = customerId;
    }
}
