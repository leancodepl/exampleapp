#if Example
using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Firebase;

[AuthorizeWhenHasAnyOf(Auth.Roles.User)]
public class SendCustomNotification : ICommand
{
    public string Content { get; set; }
    public Uri? ImageUrl { get; set; }

    public static class ErrorCodes
    {
        public const int ContentCannotBeEmpty = 1;
        public const int ImageUrlInvalid = 2;
    }
}
#endif
