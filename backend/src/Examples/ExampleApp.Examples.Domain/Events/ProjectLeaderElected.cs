using System.Text.Json.Serialization;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;

namespace ExampleApp.Examples.Domain.Events;

public class ProjectLeaderElected : IDomainEvent
{
    public Guid Id { get; private init; }
    public DateTime DateOccurred { get; private init; }

    public ProjectId ProjectId { get; private init; }
    public EmployeeId ProjectLeaderId { get; private init; }

    public ProjectLeaderElected(Project project)
    {
        Id = Guid.NewGuid();
        DateOccurred = Time.UtcNow;

        ProjectId = project.Id;
        ProjectLeaderId = project.ProjectLeaderId!.Value;
    }

    [JsonConstructor]
    public ProjectLeaderElected(Guid id, DateTime dateOccurred, ProjectId projectId, EmployeeId projectLeaderId)
    {
        Id = id;
        DateOccurred = dateOccurred;
        ProjectId = projectId;
        ProjectLeaderId = projectLeaderId;
    }
}
