using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Projects;

// [AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
[AllowUnauthorized]
public class UnassignEmployeeFromAssignment : ICommand
{
    public string AssignmentId { get; set; }

    public static class ErrorCodes
    {
        public const int AssignmentIdNotValid = 1;
        public const int ProjectWithAssignmentDoesNotExist = 2;
    }
}
