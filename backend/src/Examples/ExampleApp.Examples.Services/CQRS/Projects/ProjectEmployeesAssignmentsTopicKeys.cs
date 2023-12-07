#if Example
using ExampleApp.Examples.Contracts.Projects;
using LeanCode.Pipe;

namespace ExampleApp.Examples.Services.CQRS.Projects;

public class ProjectEmployeesAssignmentsTopicKeys
    : BasicTopicKeys<
        ProjectEmployeesAssignmentsTopic,
        EmployeeAssignedToAssignmentDTO,
        EmployeeUnassignedFromAssignmentDTO
    >
{
    public override IEnumerable<string> Get(ProjectEmployeesAssignmentsTopic topic) => [topic.ProjectId];
}
#endif
