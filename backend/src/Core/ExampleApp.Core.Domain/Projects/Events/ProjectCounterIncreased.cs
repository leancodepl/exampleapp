using System.Text.Json.Serialization;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;

namespace ExampleApp.Core.Domain.Projects.Events;

public class ProjectCounterIncreased : IDomainEvent
{
    public Guid Id { get; private init; }
    public DateTime DateOccurred { get; private init; }

    public ProjectId ProjectId { get; private init; }

    public ProjectCounterIncreased(Project project)
    {
        Id = Guid.NewGuid();
        DateOccurred = Time.NowWithOffset.UtcDateTime;

        ProjectId = project.Id;
    }

    [JsonConstructor]
    public ProjectCounterIncreased(Guid id, DateTime dateOccurred, ProjectId projectId)
    {
        Id = id;
        DateOccurred = dateOccurred;

        ProjectId = projectId;
    }
}
