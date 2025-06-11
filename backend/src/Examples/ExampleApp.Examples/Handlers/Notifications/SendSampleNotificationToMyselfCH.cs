using ExampleApp.Examples.Contracts.Notifications;
using ExampleApp.Examples.Notifications;
using LeanCode.CQRS.Execution;
using LeanCode.NotificationCenter;
using LeanCode.TimeProvider;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Handlers.Notifications;

public class SendSampleNotificationToMyselfCH : ICommandHandler<SendSampleNotificationToMyself>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<SendSampleNotificationToMyselfCH>();

    private readonly NotificationSender<Guid> notificationSender;

    public SendSampleNotificationToMyselfCH(NotificationSender<Guid> notificationSender)
    {
        this.notificationSender = notificationSender;
    }

    public async Task ExecuteAsync(HttpContext context, SendSampleNotificationToMyself command)
    {
        var userId = context.GetUserId();

        var notification = new Notification<SampleNotificationPayload, Guid>(
            NotificationId.New(),
            userId,
            "notifications.sample-notification.content",
            "notifications.sample-notification.title",
            null,
            Time.UtcNow,
            new() { ReceiversId = userId }
        );

        await notificationSender.SendAsync(notification, cancellationToken: context.RequestAborted);

        logger.Information("Sending notification {NotificationId} to user {UserId}", notification.Id, userId);
    }
}
