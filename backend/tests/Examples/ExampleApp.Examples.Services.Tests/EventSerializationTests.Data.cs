using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;
using ExampleApp.Examples.Services.DataAccess.Serialization;
using ExampleApp.Examples.Services.Processes.Kratos;
using LeanCode.Kratos.Model;
#if Example
using ExampleApp.Examples.Domain.Booking.Events;
using ExampleApp.Examples.Domain.Booking;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using ExampleApp.Examples.Domain.Projects.Events;
#endif

namespace ExampleApp.Examples.Services.Tests;

public partial class EventSerializationTests
{
#if Example
    private static readonly AssignmentId AssignmentId = AssignmentId.New();
    private static readonly EmployeeId EmployeeId = EmployeeId.New();
    private static readonly ProjectId ProjectId = ProjectId.New();
#endif
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
                },
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
                },
            ],
            MetadataAdmin = JsonSerializer.Deserialize<JsonElement>(@"{""foo"":""bar""}"),
            MetadataPublic = JsonSerializer.Deserialize<JsonElement>(@"{""roles"":[""user""]}"),
            Traits = JsonSerializer.Deserialize<JsonElement>(@"{""email"":""test@leancode.pl""}"),
        };

    private static readonly ImmutableArray<object> Events =
    [
#if Example
        new EmployeeAssignedToAssignment(Guid, DateTime, ProjectId, AssignmentId, EmployeeId, null),
        new EmployeeUnassignedFromAssignment(Guid, DateTime, ProjectId, AssignmentId, null),
        new TimeslotReserved(Guid, DateTime, CalendarDayId.New(), TimeslotId.New()),
        new TimeslotUnavailable(Guid, DateTime, CalendarDayId.New(), TimeslotId.New()),
#endif
        new KratosIdentityUpdated(Guid, DateTime, KratosIdentity),
        new KratosIdentityDeleted(Guid, DateTime, Guid),
    ];

    private static readonly ImmutableHashSet<Assembly> Assemblies =
    [
#if Example
        typeof(Employee).Assembly,
#endif
        typeof(DataAccess.ExamplesDbContext).Assembly,
    ];

    private static readonly JsonSerializerOptions SerializerOptions = KnownConverters.AddAll(new());
}
