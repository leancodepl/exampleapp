#if Example
using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Projects;

[AllowUnauthorized]
public class AddAssignmentsToProject : ICommand
{
    public string ProjectId { get; set; }
    public List<AssignmentWriteDTO> Assignments { get; set; }

    public static class ErrorCodes
    {
        public const int ProjectIdNotValid = 1;
        public const int ProjectDoesNotExist = 2;
        public const int AssignmentsCannotBeNull = 3;
        public const int AssignmentsCannotBeEmpty = 4;
    }
}

// TODO: Revise our approach to `Read` and `Write` DTOs
public class AssignmentWriteDTO
{
    public string Name { get; set; }

    public class ErrorCodes
    {
        public const int NameCannotBeEmpty = 101;
        public const int NameTooLong = 102;
    }
}
#endif
