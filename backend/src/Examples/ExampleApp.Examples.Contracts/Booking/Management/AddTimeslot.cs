using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.Management;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class AddTimeslot : ICommand
{
    public string ServiceProviderId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public MoneyDTO Price { get; set; }

    public static class ErrorCodes
    {
        public const int ServiceProviderIdIsInvalid = 1;
        public const int ServiceProviderDoesNotExist = 2;
        public const int EndTimeMustBeAfterStartTime = 3;
        public const int PriceIsNull = 4;
        public const int TimeslotOverlapsWithExisting = 5;

        public class Price : MoneyDTO.ErrorCodes { }
    }
}
