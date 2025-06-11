using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Firebase;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
[Obsolete("Use LeanCode.NotificationCenter.Contracts.RemovePushNotificationToken instead.")]
public class RemoveNotificationToken : ICommand
{
    public string Token { get; set; }

    public static class ErrorCodes
    {
        public const int TokenCannotBeEmpty = 1;
    }
}
