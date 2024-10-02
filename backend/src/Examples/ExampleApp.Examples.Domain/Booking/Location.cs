using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples.Domain.Booking;

public record Location(double Longitude, double Latitude) : ValueObject;
