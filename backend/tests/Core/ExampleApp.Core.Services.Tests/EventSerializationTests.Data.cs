using System.Text.Json;
using ExampleApp.Core.Domain.Employees;
using ExampleApp.Core.Domain.Projects;
using LeanCode.Kratos.Model;

namespace ExampleApp.Core.Services.Tests;

public partial class EventsSerializationTests
{
    private static readonly AssignmentId AssignmentId = AssignmentId.New();
    private static readonly EmployeeId EmployeeId = EmployeeId.New();
    private static readonly ProjectId ProjectId = ProjectId.New();
    private static readonly Guid Guid = Guid.NewGuid();
    private static readonly DateTime DateTime = DateTime.UtcNow;
    private static readonly Identity KratosIdentity =
        new()
        {
            State = IdentityState.Active,
            CreatedAt = DateTime,
            Id = Guid,
            RecoveryAddresses =
            [
                new()
                {
                    CreatedAt = DateTime,
                    Id = Guid,
                    UpdatedAt = DateTime,
                    Value = "test@leancode.pl",
                    Via = AddressType.Email,
                }
            ],
            SchemaId = "user",
            SchemaUrl = new("https://auth.exampleapp.test.lncd.pl/schemas/dXNlcg"),
            StateChangedAt = DateTime,
            UpdatedAt = DateTime,
            VerifiableAddresses =
            [
                new()
                {
                    CreatedAt = DateTime,
                    Id = Guid,
                    Status = AddressStatus.Completed,
                    UpdatedAt = DateTime,
                    Value = "test@leancode.pl",
                    Verified = true,
                    VerifiedAt = DateTime,
                    Via = AddressType.Email,
                }
            ],
            MetadataAdmin = JsonSerializer.Deserialize<JsonElement>(@"{""foo"":""bar""}"),
            MetadataPublic = JsonSerializer.Deserialize<JsonElement>(@"{""roles"":[""user""]}"),
            Traits = JsonSerializer.Deserialize<JsonElement>(@"{""email"":""test@leancode.pl""}"),
        };
}
