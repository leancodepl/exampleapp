using ExampleApp.Core.Contracts.Projects;
using LeanCode.Pipe;

namespace ExampleApp.Core.Services.CQRS.Projects;

public class ProjectEmployeesAssignmentsTopicKeys
    : BasicTopicKeys<
        ProjectEmployeesAssignmentsTopic,
        EmployeeAssignedToAssignmentDTO,
        EmployeeUnassignedFromAssignmentDTO
    >
{
    public override IEnumerable<string> Get(ProjectEmployeesAssignmentsTopic topic) => new[] { topic.ProjectId };
}
