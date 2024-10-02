using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples.Domain.Booking;

public record Money(decimal Value, string Currency) : ValueObject;
