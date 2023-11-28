using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;
using ExampleApp.Core.Domain.Employees;
using ExampleApp.Core.Domain.Events;
using ExampleApp.Core.Services.DataAccess.Serialization;
using ExampleApp.Core.Services.Processes.Kratos;
using FluentAssertions;
using LeanCode.DomainModels.Model;
using Xunit;

namespace ExampleApp.Core.Services.Tests;

public partial class EventsSerializationTests
{
    private static readonly JsonSerializerOptions SerializerOptions = KnownConverters.AddAll(new());

    private static readonly ImmutableHashSet<Assembly> Assemblies =
    [
        typeof(Employee).Assembly,
        typeof(DataAccess.CoreDbContext).Assembly,
    ];

    private static readonly ImmutableArray<object> Events =
    [
        new EmployeeAssignedToAssignment(Guid, DateTime, ProjectId, AssignmentId, EmployeeId, null),
        new EmployeeUnassignedFromAssignment(Guid, DateTime, ProjectId, AssignmentId, null),
        new KratosIdentityUpdated(Guid, DateTime, KratosIdentity),
        new KratosIdentityDeleted(Guid, DateTime, Guid),
    ];

    public static IEnumerable<object[]> EventTestData => Events.Select<object, object[]>(e => [e]);

    [Theory]
    [MemberData(nameof(EventTestData))]
    public void Event_correctly_roundtrips_during_STJ_serialization(object @event)
    {
        var type = @event.GetType();
        var serialized = JsonSerializer.Serialize(@event, type, SerializerOptions);
        var deserialized = JsonSerializer.Deserialize(serialized, type, SerializerOptions);

        deserialized
            .Should()
            .BeEquivalentTo(
                expectation: @event,
                config: options =>
                    options
                        .RespectingRuntimeTypes()
                        .Excluding(mi => mi.Type == typeof(JsonElement) || mi.Type == typeof(JsonElement?)), // 😢
                because: "events of type {0} should roundtrip",
                type.FullName
            );
    }

    [Fact]
    public void All_events_are_tested_for_STJ_serialization_roundtrip()
    {
        var verifiedEventTypes = Events.Select(e => e.GetType());
        var allEventTypes = Assemblies
            .SelectMany(a => a.Types())
            .ThatImplement<IDomainEvent>()
            .Where(t => !t.IsAbstract);

        allEventTypes.Should().BeSubsetOf(verifiedEventTypes);
    }
}
