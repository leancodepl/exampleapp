using System.Text.Json;
using ExampleApp.Examples.Contracts.Identities;
using ExampleApp.Examples.DataAccess;
using LeanCode.CQRS.Execution;
using LeanCode.Kratos.Client.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Handlers.Identities;

public class IdentityDetailsQH : IQueryHandler<IdentityDetails, KratosIdentityDetailsDTO?>
{
    private readonly ExamplesDbContext dbContext;
    private readonly IIdentityApi identityApi;

    public IdentityDetailsQH(ExamplesDbContext dbContext, IIdentityApi identityApi)
    {
        this.dbContext = dbContext;
        this.identityApi = identityApi;
    }

    public async Task<KratosIdentityDetailsDTO?> ExecuteAsync(HttpContext context, IdentityDetails query)
    {
        var result = await dbContext
            .KratosIdentities.AsSplitQuery()
            .Where(ki => ki.Id == query.Id)
            .Select(ki => new KratosIdentityDetailsDTO
            {
                Id = ki.Id,
                CreatedAt = ki.CreatedAt,
                UpdatedAt = ki.UpdatedAt,
                SchemaId = ki.SchemaId,
                Traits = ki.Traits,
                MetadataPublic = ki.MetadataPublic,
                MetadataAdmin = ki.MetadataAdmin,

                RecoveryAddresses = ki
                    .RecoveryAddresses.Select(a => new KratosIdentityRecoveryAddressDTO
                    {
                        Id = a.Id,
                        CreatedAt = a.CreatedAt,
                        UpdatedAt = a.UpdatedAt,
                        Via = a.Via,
                        Value = a.Value,
                    })
                    .ToList(),

                VerifiableAddresses = ki
                    .VerifiableAddresses.Select(a => new KratosIdentityVerifiableAddressDTO
                    {
                        Id = a.Id,
                        CreatedAt = a.CreatedAt,
                        UpdatedAt = a.UpdatedAt,
                        Via = a.Via,
                        Value = a.Value,
                        VerifiedAt = a.VerifiedAt,
                    })
                    .ToList(),
            })
            .FirstOrDefaultAsync(context.RequestAborted);

        if (result is null)
        {
            return null;
        }

        var traits = (JsonElement)result.Traits;
        result.Email = traits.GetProperty("email").GetString()!;
        result.GivenName = traits.GetProperty("given_name").GetString()!;
        result.FamilyName = traits.GetProperty("family_name").GetString();

        var kratosResponse = await identityApi.GetIdentityOrDefaultAsync(
            query.Id.ToString(),
            includeCredential: default,
            context.RequestAborted
        );

        if ((kratosResponse?.TryOk(out var kratosIdentity) ?? false) && kratosIdentity?.Credentials is not null)
        {
            result.Credentials = kratosIdentity
                .Credentials.Where(kvp =>
                    kvp.Value is { Type: not null, Identifiers.Count: > 0, CreatedAt: not null, UpdatedAt: not null }
                    && Enum.IsDefined((KratosIdentityCredentialTypeDTO)kvp.Value.Type)
                )
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => new KratosIdentityCredentialDTO
                    {
                        Type = (KratosIdentityCredentialTypeDTO)kvp.Value.Type.GetValueOrDefault(),
                        Identifiers = kvp.Value.Identifiers!,
                        CreatedAt = kvp.Value.CreatedAt.GetValueOrDefault(),
                        UpdatedAt = kvp.Value.UpdatedAt.GetValueOrDefault(),
                    }
                );
        }
        else
        {
            result.Credentials = [];
        }

        return result;
    }
}
