using System.Text.Json.Serialization;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;

namespace ExampleApp.Core.Domain.Projects.Events;

public class ProjectNameChanged : IDomainEvent
{
    public Guid Id { get; private init; }
    public DateTime DateOccurred { get; private init; }

    public ProjectId ProjectId { get; private init; }

    public ProjectNameChanged(Project project)
    {
        Id = Guid.NewGuid();
        DateOccurred = Time.NowWithOffset.UtcDateTime;

        ProjectId = project.Id;
    }

    [JsonConstructor]
    public ProjectNameChanged(Guid id, DateTime dateOccurred, ProjectId projectId)
    {
        Id = id;
        DateOccurred = dateOccurred;

        ProjectId = projectId;
    }
}
