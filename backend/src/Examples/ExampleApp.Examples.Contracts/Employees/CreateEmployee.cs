#if Example
using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Employees;

[AuthorizeWhenHasAnyOf(Auth.Roles.Admin)]
public class CreateEmployee : ICommand
{
    public string Name { get; set; }
    public string Email { get; set; }

    public static class ErrorCodes
    {
        public const int NameCannotBeEmpty = 1;
        public const int NameTooLong = 2;
        public const int EmailInvalid = 3;
        public const int EmailIsNotUnique = 4;
    }
}
#endif
