using ExampleApp.Examples.Contracts.Booking;
using FluentValidation;
using LeanCode.CQRS.Validation.Fluent;

namespace ExampleApp.Examples.Handlers.Booking;

public class LocationDTOValidator : AbstractValidator<LocationDTO>
{
    public LocationDTOValidator()
    {
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).WithCode(LocationDTO.ErrorCodes.LongitudeIsOutOfRange);
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).WithCode(LocationDTO.ErrorCodes.LatitudeIsOutOfRange);
    }
}
