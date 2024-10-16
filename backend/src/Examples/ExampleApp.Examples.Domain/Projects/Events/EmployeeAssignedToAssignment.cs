using System.Text.Json.Serialization;
using ExampleApp.Examples.Domain.Employees;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;

namespace ExampleApp.Examples.Domain.Projects.Events;

public class EmployeeAssignedToAssignment : IDomainEvent
{
    public Guid Id { get; private init; }
    public DateTime DateOccurred { get; private init; }

    public ProjectId ProjectId { get; private init; }
    public AssignmentId AssignmentId { get; private init; }
    public EmployeeId EmployeeId { get; private init; }
    public EmployeeId? PreviousEmployeeId { get; private init; }

    public EmployeeAssignedToAssignment(Assignment assignment, EmployeeId? previousEmployeeId)
    {
        Id = Guid.NewGuid();
        DateOccurred = Time.UtcNow;

        ProjectId = assignment.ParentProjectId;
        AssignmentId = assignment.Id;
        EmployeeId = assignment.AssignedEmployeeId!.Value;
        PreviousEmployeeId = previousEmployeeId;
    }

    [JsonConstructor]
    public EmployeeAssignedToAssignment(
        Guid id,
        DateTime dateOccurred,
        ProjectId projectId,
        AssignmentId assignmentId,
        EmployeeId employeeId,
        EmployeeId? previousEmployeeId
    )
    {
        Id = id;
        DateOccurred = dateOccurred;
        ProjectId = projectId;
        AssignmentId = assignmentId;
        EmployeeId = employeeId;
        PreviousEmployeeId = previousEmployeeId;
    }
}
