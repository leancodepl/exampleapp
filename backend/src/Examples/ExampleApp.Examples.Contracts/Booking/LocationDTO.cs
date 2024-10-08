namespace ExampleApp.Examples.Contracts.Booking;

public record LocationDTO(double Latitude, double Longitude)
{
    public class ErrorCodes
    {
        public const int LatitudeIsOutOfRange = 1;
        public const int LongitudeIsOutOfRange = 2;
    }
}
