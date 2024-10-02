using LeanCode.DomainModels.Ids;
using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples.Domain.Booking;

[TypedId(TypedIdFormat.PrefixedUlid, CustomPrefix = "timeslot")]
public readonly partial record struct TimeslotId;

public class Timeslot : IEntity<TimeslotId>
{
    public TimeslotId Id { get; private init; }

    public ServiceProviderId ServiceProviderId { get; private init; }
    public ServiceProvider ServiceProvider { get; private init; }

    public DateOnly Date { get; private init; }
    public TimeOnly StartTime { get; private init; }
    public TimeOnly EndTime { get; private init; }
    public Money Price { get; private init; }

    private Timeslot()
    {
        ServiceProvider = null!;
    }

    internal static Timeslot Create(
        ServiceProvider provider,
        DateOnly date,
        TimeOnly startTime,
        TimeOnly endTime,
        Money price
    )
    {
        if (startTime >= endTime)
        {
            throw new ArgumentException("Start time must be before end time.");
        }

        return new Timeslot
        {
            Id = TimeslotId.New(),
            ServiceProviderId = provider.Id,
            ServiceProvider = provider,
            Date = date,
            StartTime = startTime,
            EndTime = endTime,
            Price = price,
        };
    }
}
