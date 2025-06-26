using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using ExampleApp.Examples.Contracts;
using ExampleApp.Examples.Contracts.Identities;
using ExampleApp.Examples.DataAccess;
using LeanCode.CQRS.Execution;
using LeanCode.QueryableExtensions;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Examples.Handlers.Identities;

public class SearchIdentitiesQH : IQueryHandler<SearchIdentities, PaginatedResult<KratosIdentityDTO>>
{
    private readonly ExamplesDbContext dbContext;

    public SearchIdentitiesQH(ExamplesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [SuppressMessage("ReSharper", "EntityFramework.UnsupportedServerSideFunctionCall")]
    public Task<PaginatedResult<KratosIdentityDTO>> ExecuteAsync(HttpContext context, SearchIdentities query)
    {
        return dbContext
            .KratosIdentities.ConditionalWhere(ki => ki.SchemaId == query.SchemaId, query.SchemaId is not null)
            .ConditionalWhere(
                ki => Regex.IsMatch(ki.Traits.GetProperty("email").GetString()!, query.EmailPattern!),
                query.EmailPattern is not null
            )
            .ConditionalWhere(
                ki => Regex.IsMatch(ki.Traits.GetProperty("given_name").GetString()!, query.GivenNamePattern!),
                query.GivenNamePattern is not null
            )
            .ConditionalWhere(
                ki => Regex.IsMatch(ki.Traits.GetProperty("family_name").GetString()!, query.FamilyNamePattern!),
                query.FamilyNamePattern is not null
            )
            .OrderBy(ki => ki.Traits.GetProperty("email").GetString())
            .Select(ki => new KratosIdentityDTO
            {
                Id = ki.Id,
                CreatedAt = ki.CreatedAt,
                UpdatedAt = ki.UpdatedAt,
                SchemaId = ki.SchemaId,
                Email = ki.Traits.GetProperty("email").GetString()!,
                Traits = ki.Traits,
            })
            .ToPaginatedResultAsync(query, context.RequestAborted);
    }
}
