using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Projects;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class AssignEmployeeToAssignment : ICommand
{
    public string AssignmentId { get; set; }
    public string EmployeeId { get; set; }

    public static class ErrorCodes
    {
        public const int AssignmentIdNotValid = 1;
        public const int ProjectWithAssignmentDoesNotExist = 2;
        public const int EmployeeIdNotValid = 3;
        public const int EmployeeDoesNotExist = 4;
    }
}
