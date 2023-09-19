using LeanCode.Contracts;

namespace ExampleApp.Core.Contracts.Projects;

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
