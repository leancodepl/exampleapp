namespace ExampleApp.Examples.Contracts.Booking.Reservations;

public class MyReservationDTO
{
    public string Id { get; set; }
    public string TimeslotId { get; set; }
    public string ServiceProviderId { get; set; }
    public string ServiceProviderName { get; set; }

    public ServiceProviderTypeDTO Type { get; set; }
    public string Address { get; set; }
    public LocationDTO Location { get; set; }

    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public MoneyDTO Price { get; set; }

    public ReservationStatusDTO Status { get; set; }
}

public enum ReservationStatusDTO
{
    Pending = 0,
    Confirmed = 1,
    Rejected = 2,
    Paid = 3,
}
