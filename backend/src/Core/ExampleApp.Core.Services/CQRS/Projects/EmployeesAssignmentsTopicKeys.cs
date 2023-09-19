using ExampleApp.Core.Contracts.Projects;
using LeanPipe;

namespace ExampleApp.Core.Services.CQRS.Projects;

public class EmployeeAssignmentsTopicKeys : BasicTopicKeys<EmployeeAssignmentsTopic>
{
    public override IEnumerable<string> Get(EmployeeAssignmentsTopic topic) => new[] { topic.EmployeeId };
}
