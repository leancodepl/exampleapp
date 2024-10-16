using ExampleApp.Examples.Contracts.Firebase;
using FirebaseAdmin.Messaging;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.Firebase.FCM;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Services.Handlers.Firebase;

public class SendCustomNotificationCV : AbstractValidator<SendCustomNotification>
{
    public SendCustomNotificationCV()
    {
        RuleFor(cmd => cmd.Content).NotEmpty().WithCode(SendCustomNotification.ErrorCodes.ContentCannotBeEmpty);

        RuleFor(cmd => cmd.ImageUrl)
            .Must(uri => uri?.IsAbsoluteUri ?? true)
            .WithCode(SendCustomNotification.ErrorCodes.ImageUrlInvalid);
    }
}

public class SendCustomNotificationCH : ICommandHandler<SendCustomNotification>
{
    private readonly FCMClient<Guid> fcmClient;

    public SendCustomNotificationCH(FCMClient<Guid> fcmClient)
    {
        this.fcmClient = fcmClient;
    }

    public async Task ExecuteAsync(HttpContext context, SendCustomNotification command)
    {
        var userId = context.GetUserId();
        var message = new MulticastMessage
        {
            // TODO: localize?
            Notification = fcmClient
                .Localize(Consts.DefaultUserCulture)
                .Title("notifications.meeting-started.title")
                .Body("notifications.meeting-started.body", command.Content)
                .RawImageUrl(command.ImageUrl?.AbsoluteUri!)
                .Build(),
            Data = new Dictionary<string, string>
            {
                ["click_action"] = "FLUTTER_NOTIFICATION_CLICK",
                ["type"] = "MeetingHasStarted",
                ["name"] = command.Content,
            },
        };

        await fcmClient.SendToUserAsync(userId, message, context.RequestAborted);
    }
}
