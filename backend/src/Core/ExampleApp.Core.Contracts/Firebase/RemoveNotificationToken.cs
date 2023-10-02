using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Core.Contracts.Firebase;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
public class RemoveNotificationToken : ICommand
{
    public string Token { get; set; }

    public static class ErrorCodes
    {
        public const int TokenCannotBeEmpty = 1;
    }
}
