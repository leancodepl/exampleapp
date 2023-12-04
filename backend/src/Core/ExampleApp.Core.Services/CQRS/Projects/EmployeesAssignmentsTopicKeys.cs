using ExampleApp.Core.Contracts.Projects;
using LeanCode.Pipe;

namespace ExampleApp.Core.Services.CQRS.Projects;

public class EmployeeAssignmentsTopicKeys
    : BasicTopicKeys<
        EmployeeAssignmentsTopic,
        EmployeeAssignedToProjectAssignmentDTO,
        EmployeeUnassignedFromProjectAssignmentDTO
    >
{
    public override IEnumerable<string> Get(EmployeeAssignmentsTopic topic) => [topic.EmployeeId];
}
