using LeanCode.NotificationCenter.Configuration;

namespace ExampleApp.Examples.IntegrationTests.Helpers;

public class NotificationsUserConfigurationProviderMock : IUserConfigurationProvider<Guid>
{
    public NotificationsUserConfigurationProviderMock() { }

    public Task<UserConfiguration> GetConfigurationAsync(Guid userId, CancellationToken cancellationToken) =>
        Task.FromResult<UserConfiguration>(new(userId.ToString(), "pl", "mock.email@leancode.pl"));
}
