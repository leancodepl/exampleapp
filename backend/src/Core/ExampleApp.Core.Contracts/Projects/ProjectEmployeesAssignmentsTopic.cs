using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Core.Contracts.Projects;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class ProjectEmployeesAssignmentsTopic
    : ITopic,
        IProduceNotification<EmployeeAssignedToAssignmentDTO>,
        IProduceNotification<EmployeeUnassignedFromAssignmentDTO>
{
    public string ProjectId { get; set; }
}

public class EmployeeAssignedToAssignmentDTO
{
    public string AssignmentId { get; set; }
    public string EmployeeId { get; set; }
}

public class EmployeeUnassignedFromAssignmentDTO
{
    public string AssignmentId { get; set; }
}
