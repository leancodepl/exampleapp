using System.Text.Json.Serialization;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;

namespace ExampleApp.Examples.Domain.Booking.Events;

public class TimeslotUnavailable : IDomainEvent
{
    public Guid Id { get; }
    public DateTime DateOccurred { get; }

    public ServiceProviderId ServiceProviderId { get; }
    public TimeslotId TimeslotId { get; }

    public TimeslotUnavailable(ServiceProviderId serviceProviderId, TimeslotId timeslotId)
    {
        Id = Guid.NewGuid();
        DateOccurred = Time.UtcNow;

        ServiceProviderId = serviceProviderId;
        TimeslotId = timeslotId;
    }

    [JsonConstructor]
    public TimeslotUnavailable(
        Guid id,
        DateTime dateOccurred,
        ServiceProviderId serviceProviderId,
        TimeslotId timeslotId
    )
    {
        Id = id;
        DateOccurred = dateOccurred;
        ServiceProviderId = serviceProviderId;
        TimeslotId = timeslotId;
    }
}
