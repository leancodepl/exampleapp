using System.Text.Json.Serialization;
using ExampleApp.Core.Domain.Employees;
using ExampleApp.Core.Domain.Projects;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;

namespace ExampleApp.Core.Domain.Events;

public class EmployeeUnassignedFromAssignment : IDomainEvent
{
    public Guid Id { get; private init; }
    public DateTime DateOccurred { get; private init; }

    public ProjectId ProjectId { get; private init; }
    public AssignmentId AssignmentId { get; private init; }

    public EmployeeUnassignedFromAssignment(Project project, AssignmentId assignmentId)
    {
        Id = Guid.NewGuid();
        DateOccurred = Time.NowWithOffset.UtcDateTime;

        ProjectId = project.Id;
        AssignmentId = assignmentId;
    }

    [JsonConstructor]
    public EmployeeUnassignedFromAssignment(
        Guid id,
        DateTime dateOccurred,
        ProjectId projectId,
        AssignmentId assignmentId
    )
    {
        Id = id;
        DateOccurred = dateOccurred;
        ProjectId = projectId;
        AssignmentId = assignmentId;
    }
}
