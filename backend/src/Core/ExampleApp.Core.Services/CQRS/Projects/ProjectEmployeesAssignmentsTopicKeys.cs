using ExampleApp.Core.Contracts.Projects;
using LeanPipe;

namespace ExampleApp.Core.Services.CQRS.Projects;

public class ProjectEmployeesAssignmentsTopicKeys : BasicTopicKeys<ProjectEmployeesAssignmentsTopic>
{
    public override IEnumerable<string> Get(ProjectEmployeesAssignmentsTopic topic) => new[] { topic.ProjectId };
}
