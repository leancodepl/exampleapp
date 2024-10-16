using ExampleApp.Examples.DataAccess;
using LeanCode.DomainModels.Model;
using LeanCode.Kratos.Model;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Handlers.Identities;

public sealed record class KratosIdentityUpdated(Guid Id, DateTime DateOccurred, Identity Identity) : IDomainEvent;

public sealed record class KratosIdentityDeleted(Guid Id, DateTime DateOccurred, Guid IdentityId) : IDomainEvent;

public class SyncKratosIdentityEH : IConsumer<KratosIdentityUpdated>, IConsumer<KratosIdentityDeleted>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<SyncKratosIdentityEH>();

    private readonly ExamplesDbContext dbContext;

    public SyncKratosIdentityEH(ExamplesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<KratosIdentityUpdated> context)
    {
        var kratosIdentity = context.Message.Identity;
        var identityId = kratosIdentity.Id;

        var dbIdentity = await dbContext.KratosIdentities.FindAsync(keyValues: [identityId], context.CancellationToken);

        if (dbIdentity is null)
        {
            dbIdentity = new(kratosIdentity);
            dbContext.KratosIdentities.Add(dbIdentity);

            logger.Information("Identity {IdentityId} replicated", identityId);
        }
        else
        {
            dbIdentity.Update(kratosIdentity);
            dbContext.KratosIdentities.Update(dbIdentity);

            logger.Information("Replica of Identity {IdentityId} updated", identityId);
        }
    }

    public async Task Consume(ConsumeContext<KratosIdentityDeleted> context)
    {
        var identityId = context.Message.IdentityId;

        var deleted = await dbContext
            .KratosIdentities.Where(ki => ki.Id == identityId)
            .ExecuteDeleteAsync(context.CancellationToken);

        if (deleted == 0)
        {
            logger.Information("Replica of Identity {IdentityId} could not be found, nothing to do here", identityId);
        }
        else
        {
            logger.Information("Replica of Identity {IdentityId} deleted", identityId);
        }
    }
}
