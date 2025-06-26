using ExampleApp.Examples.DataAccess;
using LeanCode.NotificationCenter.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Notifications;

public class NotificationsUserConfigurationProvider : IUserConfigurationProvider<Guid>
{
    private readonly ExamplesDbContext dbContext;

    public NotificationsUserConfigurationProvider(ExamplesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<UserConfiguration> GetConfigurationAsync(Guid userId, CancellationToken cancellationToken)
    {
        var stringId = userId.ToString();
        var identity = await dbContext
            .KratosIdentities.Where(ki => ki.Id == userId)
            .Select(ki => new { Email = ki.Traits.GetProperty("email").GetString()! })
            .FirstAsync(cancellationToken);

        return new(stringId, "pl", identity.Email);
    }
}
