using ExampleApp.Core.Services.Processes.Kratos;
using LeanCode.Firebase.FCM;
using MassTransit;

namespace ExampleApp.Core.Services.Processes.Firebase;

public class RemoveNotificationTokens : IConsumer<KratosIdentityDeleted>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<RemoveNotificationTokens>();

    private readonly IPushNotificationTokenStore<Guid> pushNotificationTokenStore;

    public RemoveNotificationTokens(IPushNotificationTokenStore<Guid> pushNotificationTokenStore)
    {
        this.pushNotificationTokenStore = pushNotificationTokenStore;
    }

    public async Task Consume(ConsumeContext<KratosIdentityDeleted> context)
    {
        var userId = context.Message.IdentityId;
        var tokens = await pushNotificationTokenStore.GetTokensAsync(userId, context.CancellationToken);

        if (tokens.Count > 0)
        {
            await pushNotificationTokenStore.RemoveTokensAsync(tokens, context.CancellationToken);
            logger.Information(
                "Removed {Count} notification tokens belonging to deleted Identity {IdentityId}",
                tokens.Count,
                userId
            );
        }
        else
        {
            logger.Information(
                "There are no notification tokens belonging to deleted Identity {IdentityId} to remove",
                userId
            );
        }
    }
}
