using LeanCode.DomainModels.Ids;

namespace ExampleApp.Examples.Domain;

[TypedId(TypedIdFormat.RawGuid)]
public readonly partial record struct CustomerId;
