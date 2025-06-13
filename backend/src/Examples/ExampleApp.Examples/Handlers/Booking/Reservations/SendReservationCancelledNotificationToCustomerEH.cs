using ExampleApp.Examples.Contracts.Booking.Reservations;
using ExampleApp.Examples.DataAccess;
using ExampleApp.Examples.Domain.Booking.Events;
using LeanCode.NotificationCenter;
using LeanCode.TimeProvider;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Handlers.Booking.Reservations;

public class SendReservationCancelledNotificationToCustomerEH(
    ExamplesDbContext dbContext,
    NotificationSender<Guid> notificationSender
) : IConsumer<ReservationCancelled>
{
    private readonly Serilog.ILogger logger =
        Serilog.Log.ForContext<SendReservationCancelledNotificationToCustomerEH>();

    public async Task Consume(ConsumeContext<ReservationCancelled> context)
    {
        var msg = context.Message;

        var notificationData = await dbContext
            .Timeslots.Where(t => t.Id == msg.TimeslotId)
            .Join(
                dbContext.ServiceProviders,
                t => t.CalendarDay.ServiceProviderId,
                sp => sp.Id,
                (t, sp) =>
                    new
                    {
                        t.Date,
                        t.StartTime,
                        t.EndTime,
                        ServiceProviderId = sp.Id,
                        ServiceProviderName = sp.Name,
                        ServiceProviderThumbnail = sp.Thumbnail,
                    }
            )
            .FirstAsync(context.CancellationToken);

        var notification = new Notification<ReservationCancelledNotificationDTO, Guid>(
            NotificationId.New(),
            msg.CustomerId,
            "notifications.reservation-cancelled.content",
            "notifications.reservation-cancelled.title",
            notificationData.ServiceProviderThumbnail,
            Time.UtcNow,
            new()
            {
                ReservationId = msg.ReservationId,
                CalendarDayId = msg.CalendarDayId,
                TimeslotId = msg.TimeslotId,
                ServiceProviderId = notificationData.ServiceProviderId,
                ServiceProviderName = notificationData.ServiceProviderName,
                Date = notificationData.Date,
                StartTime = notificationData.StartTime,
                EndTime = notificationData.EndTime,
            }
        );

        await notificationSender.SendAsync(notification, cancellationToken: context.CancellationToken);

        logger.Information(
            "Sending reservation cancelled notification {NotificationId} about reservation {ReservationId} to user {UserId}",
            notification.Id,
            msg.ReservationId,
            msg.CustomerId
        );
    }
}
