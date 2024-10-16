using ExampleApp.Examples.Contracts.Users;
using ExampleApp.Examples.Services.Processes.Kratos;
using LeanCode.CQRS.Execution;
using LeanCode.Kratos.Client.Api;
using LeanCode.TimeProvider;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Services.Handlers.Users;

public class DeleteOwnAccountCH : ICommandHandler<DeleteOwnAccount>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<DeleteOwnAccountCH>();

    private readonly IIdentityApi identityApi;
    private readonly IBus bus;

    public DeleteOwnAccountCH(IIdentityApi identityApi, IBus bus)
    {
        this.identityApi = identityApi;
        this.bus = bus;
    }

    public async Task ExecuteAsync(HttpContext context, DeleteOwnAccount command)
    {
        var userId = context.GetUserId();

        var deleted = await identityApi.DeleteIdentityAsync(userId.ToString(), context.RequestAborted);

        if (!deleted.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Failed to delete Kratos identity.");
        }

        await bus.Publish(new KratosIdentityDeleted(Guid.NewGuid(), Time.UtcNow, userId), context.RequestAborted);

        logger.Information("User account {UserId} has been deleted", userId);
    }
}
