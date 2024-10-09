using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples.Domain.Booking;

public record Location(double Latitude, double Longitude) : ValueObject;
