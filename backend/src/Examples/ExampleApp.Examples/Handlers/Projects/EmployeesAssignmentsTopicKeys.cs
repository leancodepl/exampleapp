using ExampleApp.Examples.Contracts.Projects;
using LeanCode.Pipe;

namespace ExampleApp.Examples.Handlers.Projects;

public class EmployeeAssignmentsTopicKeys
    : BasicTopicKeys<
        EmployeeAssignmentsTopic,
        EmployeeAssignedToProjectAssignmentDTO,
        EmployeeUnassignedFromProjectAssignmentDTO
    >
{
    public override IEnumerable<string> Get(EmployeeAssignmentsTopic topic) => [topic.EmployeeId];
}
