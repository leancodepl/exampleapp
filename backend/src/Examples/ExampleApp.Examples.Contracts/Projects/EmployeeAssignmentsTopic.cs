using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Projects;

[AllowUnauthorized]
public class EmployeeAssignmentsTopic
    : ITopic,
        IProduceNotification<EmployeeAssignedToProjectAssignmentDTO>,
        IProduceNotification<EmployeeUnassignedFromProjectAssignmentDTO>
{
    public string EmployeeId { get; set; }
}

public class EmployeeAssignedToProjectAssignmentDTO
{
    public string ProjectId { get; set; }
    public string AssignmentId { get; set; }
}

public class EmployeeUnassignedFromProjectAssignmentDTO
{
    public string ProjectId { get; set; }
    public string AssignmentId { get; set; }
}
