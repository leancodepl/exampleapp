using ExampleApp.Examples.Contracts.Firebase;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.Firebase.FCM;
using Microsoft.AspNetCore.Http;
#pragma warning disable CS0618 // Type or member is obsolete

namespace ExampleApp.Examples.Handlers.Firebase;

public class RemoveNotificationTokenCV : AbstractValidator<RemoveNotificationToken>
{
    public RemoveNotificationTokenCV()
    {
        RuleFor(cmd => cmd.Token).NotEmpty().WithCode(RemoveNotificationToken.ErrorCodes.TokenCannotBeEmpty);
    }
}

public class RemoveNotificationTokenCH : ICommandHandler<RemoveNotificationToken>
{
    private readonly IPushNotificationTokenStore<Guid> pushNotificationTokenStore;

    public RemoveNotificationTokenCH(IPushNotificationTokenStore<Guid> pushNotificationTokenStore)
    {
        this.pushNotificationTokenStore = pushNotificationTokenStore;
    }

    public Task ExecuteAsync(HttpContext context, RemoveNotificationToken command)
    {
        return pushNotificationTokenStore.RemoveUserTokenAsync(
            context.GetUserId(),
            command.Token,
            context.RequestAborted
        );
    }
}
