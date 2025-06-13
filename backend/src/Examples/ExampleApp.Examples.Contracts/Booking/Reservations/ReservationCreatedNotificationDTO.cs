namespace ExampleApp.Examples.Contracts.Booking.Reservations;

public class ReservationCreatedNotificationDTO
{
    public string ReservationId { get; set; }
    public string CalendarDayId { get; set; }
    public string TimeslotId { get; set; }
    public string ServiceProviderId { get; set; }
    public string ServiceProviderName { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
