using LeanCode.Kratos.Client.Api;
using LeanCode.NotificationCenter.Configuration;

namespace ExampleApp.Examples.Services;

public class NotificationsUserConfigurationProvider : IUserConfigurationProvider<Guid>
{
    private readonly IIdentityApi identityApi;

    public NotificationsUserConfigurationProvider(IIdentityApi identityApi)
    {
        this.identityApi = identityApi;
    }

    public async Task<UserConfiguration> GetConfigurationAsync(Guid userId, CancellationToken cancellationToken)
    {
        var stringId = userId.ToString();
        var identity = (await identityApi.GetIdentityAsync(stringId, cancellationToken: cancellationToken)).Ok();

        var email = Newtonsoft
            .Json.Linq.JObject.FromObject(identity!.Traits!)
            .Value<string>("email")
            ?.ToLowerInvariant();

        return new(stringId, "pl", email!);
    }
}
