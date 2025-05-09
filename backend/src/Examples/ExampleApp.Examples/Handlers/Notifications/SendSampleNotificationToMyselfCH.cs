using ExampleApp.Examples.Contracts.Notifications;
using ExampleApp.Examples.Handlers.Identities;
using ExampleApp.Examples.Notifications;
using LeanCode.CQRS.Execution;
using LeanCode.NotificationCenter;
using LeanCode.TimeProvider;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Handlers.Notifications;

public class SendSampleNotificationToMyselfCH : ICommandHandler<SendSampleNotificationToMyself>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<SendSampleNotificationToMyselfCH>();

    private readonly MessageSender<Guid> messageSender;

    public SendSampleNotificationToMyselfCH(MessageSender<Guid> messageSender)
    {
        this.messageSender = messageSender;
    }

    public async Task ExecuteAsync(HttpContext context, SendSampleNotificationToMyself command)
    {
        var userId = context.GetUserId();

        var message = new Message<SampleNotificationPayload, Guid>(
            MessageId.New(),
            userId,
            "notifications.sample-notification.content",
            "notifications.sample-notification.title",
            null,
            Time.UtcNow,
            new() { ReceiversId = userId });

        await messageSender.SendAsync(message, context.RequestAborted);

        logger.Information("Sending message {MessageId} to user {UserId}", message.Id, userId);
    }
}
