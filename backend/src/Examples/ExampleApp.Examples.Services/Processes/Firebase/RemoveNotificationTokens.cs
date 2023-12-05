using ExampleApp.Examples.Services.Processes.Kratos;
using LeanCode.Firebase.FCM;
using MassTransit;

namespace ExampleApp.Examples.Services.Processes.Firebase;

public class RemoveNotificationTokens : IConsumer<KratosIdentityDeleted>
{
    private readonly IPushNotificationTokenStore<Guid> pushNotificationTokenStore;

    public RemoveNotificationTokens(IPushNotificationTokenStore<Guid> pushNotificationTokenStore)
    {
        this.pushNotificationTokenStore = pushNotificationTokenStore;
    }

    public Task Consume(ConsumeContext<KratosIdentityDeleted> context)
    {
        return pushNotificationTokenStore.RemoveAllUserTokensAsync(
            context.Message.IdentityId,
            context.CancellationToken
        );
    }
}
