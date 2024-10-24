using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.Reservations;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
public class ReserveTimeslot : ICommand
{
    public string CalendarDayId { get; set; }
    public string TimeslotId { get; set; }

    public static class ErrorCodes
    {
        public const int TimeslotIdInvalid = 1;
        public const int TimeslotCannotBeReserved = 2;
        public const int CalendarDayIdInvalid = 3;
        public const int CalendarDayDoesNotExist = 4;
    }
}
