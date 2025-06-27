using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using ExampleApp.Examples.Contracts;
using ExampleApp.Examples.Contracts.Identities;
using ExampleApp.Examples.DataAccess;
using ExampleApp.Examples.DataAccess.Entities;
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
            .OrderBy(query)
            .Select(ki => new KratosIdentityDTO
            {
                Id = ki.Id,
                CreatedAt = ki.CreatedAt,
                UpdatedAt = ki.UpdatedAt,
                SchemaId = ki.SchemaId,
                Email = ki.Traits.GetProperty("email").GetString()!,
                GivenName = ki.Traits.GetProperty("given_name").GetString()!,
                FamilyName = ki.Traits.GetProperty("family_name").GetString(),
                Traits = ki.Traits,
                MetadataPublic = ki.MetadataPublic,
                MetadataAdmin = ki.MetadataAdmin,
            })
            .ToPaginatedResultAsync(query, context.RequestAborted);
    }
}

file static class Extensions
{
    public static IOrderedQueryable<KratosIdentity> OrderBy(
        this IQueryable<KratosIdentity> queryable,
        SearchIdentities query
    )
    {
        var ordered = query.SortBy switch
        {
            KratosIdentitySortKeyDTO.Email => queryable.OrderBy(
                ki => ki.Traits.GetProperty("email").GetString()!,
                query.SortByDescending
            ),
            KratosIdentitySortKeyDTO.GivenName => queryable.OrderBy(
                ki => ki.Traits.GetProperty("given_name").GetString()!,
                query.SortByDescending
            ),
            KratosIdentitySortKeyDTO.FamilyName => queryable.OrderBy(
                ki => ki.Traits.GetProperty("family_name").GetString()!,
                query.SortByDescending
            ),
            KratosIdentitySortKeyDTO.CreatedAt or _ => queryable.OrderBy(ki => ki.CreatedAt, query.SortByDescending),
        };

        return ordered.ThenBy(ki => ki.Id);
    }
}
