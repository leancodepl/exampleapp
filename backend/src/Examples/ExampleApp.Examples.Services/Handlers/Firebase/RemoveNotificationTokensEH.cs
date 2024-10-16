using ExampleApp.Examples.Services.Handlers.Identities;
using LeanCode.Firebase.FCM;
using MassTransit;

namespace ExampleApp.Examples.Services.Handlers.Firebase;

public class RemoveNotificationTokensEH : IConsumer<KratosIdentityDeleted>
{
    private readonly IPushNotificationTokenStore<Guid> pushNotificationTokenStore;

    public RemoveNotificationTokensEH(IPushNotificationTokenStore<Guid> pushNotificationTokenStore)
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
