using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Core.Contracts.Projects;

[AllowUnauthorized]
public class CreateProjectOperation : IOperation<bool>
{
    public string Name { get; set; }

    public static class ErrorCodes
    {
        public const int NameCannotBeEmpty = 1;
        public const int NameTooLong = 2;
    }
}