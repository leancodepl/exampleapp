using ExampleApp.Examples.Contracts.Firebase;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.Firebase.FCM;
using Microsoft.AspNetCore.Http;
#pragma warning disable CS0618 // Type or member is obsolete

namespace ExampleApp.Examples.Handlers.Firebase;

public class AddNotificationTokenCV : AbstractValidator<AddNotificationToken>
{
    public AddNotificationTokenCV()
    {
        RuleFor(cmd => cmd.Token).NotEmpty().WithCode(AddNotificationToken.ErrorCodes.TokenCannotBeEmpty);
    }
}

public class AddNotificationTokenCH : ICommandHandler<AddNotificationToken>
{
    private readonly IPushNotificationTokenStore<Guid> pushNotificationTokenStore;

    public AddNotificationTokenCH(IPushNotificationTokenStore<Guid> pushNotificationTokenStore)
    {
        this.pushNotificationTokenStore = pushNotificationTokenStore;
    }

    public Task ExecuteAsync(HttpContext context, AddNotificationToken command)
    {
        return pushNotificationTokenStore.AddUserTokenAsync(context.GetUserId(), command.Token, context.RequestAborted);
    }
}
