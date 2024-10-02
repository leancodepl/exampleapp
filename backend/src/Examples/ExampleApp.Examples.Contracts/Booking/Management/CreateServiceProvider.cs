using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Booking.Management;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class CreateServiceProvider : ICommand
{
    public string Name { get; set; }
    public ServiceProviderTypeDTO Type { get; set; }
    public string Description { get; set; }
    public Uri PromotionalBanner { get; set; }
    public Uri ListItemPicture { get; set; }
    public string Address { get; set; }
    public LocationDTO Location { get; set; }

    public static class ErrorCodes
    {
        public const int NameIsNullOrEmpty = 1;
        public const int NameIsTooLong = 2;
        public const int TypeIsNullOrInvalid = 3;
        public const int DescriptionIsNullOrEmpty = 4;
        public const int DescriptionIsTooLong = 5;
        public const int PromotionalBannerIsInvalid = 6;
        public const int ListItemPictureIsInvalid = 7;
        public const int AddressIsNullOrEmpty = 8;
        public const int AddressIsTooLong = 9;
        public const int LocationIsInvalid = 10;
    }
}
