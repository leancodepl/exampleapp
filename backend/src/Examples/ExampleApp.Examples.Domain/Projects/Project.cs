using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Events;
using LeanCode.DomainModels.Ids;
using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples.Domain.Projects;

[TypedId(TypedIdFormat.PrefixedUlid, CustomPrefix = "project")]
public readonly partial record struct ProjectId;

public class Project : IAggregateRoot<ProjectId>
{
    private readonly List<Assignment> assignments = new();

    public ProjectId Id { get; private init; }
    public string Name { get; private set; } = default!;
    public EmployeeId? ProjectLeaderId { get; private set; }

    public IReadOnlyList<Assignment> Assignments => assignments;

    DateTime IOptimisticConcurrency.DateModified { get; set; }

    private Project() { }

    public static Project Create(string name)
    {
        return new Project { Id = ProjectId.New(), Name = name, };
    }

    public void AddAssignments(IEnumerable<string> assignmentNames)
    {
        assignments.AddRange(assignmentNames.Select(an => Assignment.Create(this, an)));
    }

    public void EditAssignment(AssignmentId assignmentId, string name)
    {
        assignments.Single(t => t.Id == assignmentId).Edit(name);
    }

    public void AssignEmployeeToAssignment(AssignmentId assignmentId, EmployeeId employeeId)
    {
        var assignment = assignments.Single(t => t.Id == assignmentId);
        var previousEmployeeId = assignment.AssignedEmployeeId;

        assignment.AssignEmployee(employeeId);
        DomainEvents.Raise(new EmployeeAssignedToAssignment(assignment, previousEmployeeId));
    }

    public void UnassignEmployeeFromAssignment(AssignmentId assignmentId)
    {
        var assignment = assignments.Single(t => t.Id == assignmentId);
        var previousEmployeeId = assignment.AssignedEmployeeId;

        assignment.UnassignEmployee();
        DomainEvents.Raise(new EmployeeUnassignedFromAssignment(assignment, previousEmployeeId));
    }

    public void ElectProjectLeader(EmployeeId projectLeaderId)
    {
        if (!Assignments.Any(a => a.AssignedEmployeeId == projectLeaderId))
        {
            throw new InvalidOperationException("Employee with no assignments cannot be the project leader.");
        }

        ProjectLeaderId = projectLeaderId;
    }

    public void ChangeAssignmentStatus(AssignmentId assignmentId, Assignment.AssignmentStatus status)
    {
        assignments.Single(t => t.Id == assignmentId).ChangeStatus(status);
    }
}
