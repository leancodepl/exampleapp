using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Core.Contracts.Firebase;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
public class SendCustomNotification : ICommand
{
    public string Content { get; set; }

    public static class ErrorCodes
    {
        public const int ContentCannotBeEmpty = 1;
    }
}
