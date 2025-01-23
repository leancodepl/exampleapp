using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.Reservations.Authorization;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class AuthorizeWhenOwnsReservationAttribute
    : AuthorizeWhenAttribute<AuthorizeWhenOwnsReservationAttribute.IWhenOwnsReservation>
{
    public interface IWhenOwnsReservation { }

    public interface IReservationRelated
    {
        string ReservationId { get; }
    }
}
