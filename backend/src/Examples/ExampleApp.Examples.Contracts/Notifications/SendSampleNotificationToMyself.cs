using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace ExampleApp.Examples.Contracts.Notifications;

[AuthorizeWhenHasAnyOf(LeanCode.NotificationCenter.Contracts.Permissions.NotificationCenter)]
public class SendSampleNotificationToMyself : ICommand { }
