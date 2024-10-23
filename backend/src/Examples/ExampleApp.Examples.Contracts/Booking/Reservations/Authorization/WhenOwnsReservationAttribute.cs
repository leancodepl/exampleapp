using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.Reservations.Authorization;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class WhenOwnsReservationAttribute : AuthorizeWhenAttribute<WhenOwnsReservationAttribute.IWhenOwnsReservation>
{
    public interface IWhenOwnsReservation { }

    public interface IReservationRelated
    {
        public string ReservationId { get; }
    }
}
