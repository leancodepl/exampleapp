using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Core.Contracts.Projects;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class UnassignEmployeeFromAssignment : ICommand
{
    public string AssignmentId { get; set; }

    public static class ErrorCodes
    {
        public const int AssignmentIdNotValid = 1;
        public const int ProjectWithAssignmentDoesNotExist = 2;
    }
}
