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
        public const int ServiceProviderIdIsNullOrEmpty = 1;
        public const int DateIsInvalid = 2;
        public const int StartTimeIsInvalid = 3;
        public const int EndTimeIsInvalid = 4;
        public const int EndTimeMustBeAfterStartTime = 5;
        public const int PriceIsNull = 6;
        public const int PriceCurrencyIsInvalid = 7;
        public const int TimeslotOverlapsWithExisting = 8;
    }
}
