using ExampleApp.Core.Domain.Employees;
using ExampleApp.Core.Domain.Events;
using ExampleApp.Core.Domain.Projects.Events;
using LeanCode.DomainModels.Ids;
using LeanCode.DomainModels.Model;

namespace ExampleApp.Core.Domain.Projects;

[TypedId(TypedIdFormat.PrefixedUlid, CustomPrefix = "project")]
public readonly partial record struct ProjectId;

public class Project : IAggregateRoot<ProjectId>
{
    private readonly List<Assignment> assignments = new();

    private readonly List<OwnedEntity> ownedEntities = new();
    private readonly List<IncludedEntity> includedEntities = new();

    public ProjectId Id { get; private init; }
    public string Name { get; private set; } = default!;

    public IReadOnlyList<Assignment> Assignments => assignments;
    public IReadOnlyList<OwnedEntity> OwnedEntities => ownedEntities;
    public IReadOnlyList<IncludedEntity> IncludedEntities => includedEntities;

    DateTime IOptimisticConcurrency.DateModified { get; set; }

    private Project() { }

    public static Project Create(string name)
    {
        var p = new Project { Id = ProjectId.New(), Name = name, };

        DomainEvents.Raise(new ProjectCreated(p));

        return p;
    }

    public void AddTestEntities()
    {
        foreach (var i in Enumerable.Range(20, 5))
        {
            ownedEntities.Add(new OwnedEntity(i, i.ToString()));
            includedEntities.Add(new IncludedEntity(i, i.ToString()));
        }
    }

    public void UpdateTestEntities()
    {
        foreach (var e in ownedEntities)
        {
            e.Update(e.SomeInt, (e.SomeInt + 100).ToString());
        }
        foreach (var e in includedEntities)
        {
            e.Update(e.SomeInt, (e.SomeInt + 100).ToString());
        }
    }

    public void ChangeName(string newName)
    {
        Name = newName;

        DomainEvents.Raise(new ProjectNameChanged(this));
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

    public void ChangeAssignmentStatus(AssignmentId assignmentId, Assignment.AssignmentStatus status)
    {
        assignments.Single(t => t.Id == assignmentId).ChangeStatus(status);
    }
}
