namespace ExampleApp.Core.Contracts.Firebase;

#pragma warning disable IDE1006
public abstract class PushNotificationDataDTO
{
    public string click_action { get; set; }
    public string type { get; set; }
    public string data { get; set; }
}
#pragma warning restore IDE1006

public static class Notifications
{
    public interface INotificationDTO { }

    public class CustomNotificationDTO : INotificationDTO
    {
        public Guid UserId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Content { get; set; }
    }
}
