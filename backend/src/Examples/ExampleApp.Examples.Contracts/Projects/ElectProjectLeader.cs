using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Projects;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class ElectProjectLeader : ICommand
{
    public string ProjectId { get; set; }
    public string EmployeeId { get; set; }

    public static class ErrorCodes
    {
        public const int ProjectDoesNotExist = 1;
        public const int EmployeeDoesNotExist = 2;
        public const int EmployeeMustBeAssignedToAnyAssignment = 3;
    }
}
