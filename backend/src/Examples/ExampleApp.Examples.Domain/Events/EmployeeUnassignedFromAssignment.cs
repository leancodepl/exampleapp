using System.Text.Json.Serialization;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;

namespace ExampleApp.Examples.Domain.Events;

public class EmployeeUnassignedFromAssignment : IDomainEvent
{
    public Guid Id { get; private init; }
    public DateTime DateOccurred { get; private init; }

    public ProjectId ProjectId { get; private init; }
    public AssignmentId AssignmentId { get; private init; }
    public EmployeeId? PreviousEmployeeId { get; private init; }

    public EmployeeUnassignedFromAssignment(Assignment assignment, EmployeeId? previousEmployeeId)
    {
        Id = Guid.NewGuid();
        DateOccurred = Time.NowWithOffset.UtcDateTime;

        ProjectId = assignment.ParentProjectId;
        AssignmentId = assignment.Id;
        PreviousEmployeeId = previousEmployeeId;
    }

    [JsonConstructor]
    public EmployeeUnassignedFromAssignment(
        Guid id,
        DateTime dateOccurred,
        ProjectId projectId,
        AssignmentId assignmentId,
        EmployeeId? previousEmployeeId
    )
    {
        Id = id;
        DateOccurred = dateOccurred;
        ProjectId = projectId;
        AssignmentId = assignmentId;
        PreviousEmployeeId = previousEmployeeId;
    }
}
