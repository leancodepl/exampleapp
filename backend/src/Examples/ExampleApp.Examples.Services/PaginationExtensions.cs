using ExampleApp.Examples.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services;

public static class PaginationExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(
        this IQueryable<T> queryable,
        PaginatedQuery<T> query,
        CancellationToken cancellationToken = default
    )
    {
        var takeItems = Math.Clamp(query.PageSize, PaginatedQuery<T>.MinPageSize, PaginatedQuery<T>.MaxPageSize);

        var count = await queryable.CountAsync(cancellationToken);

        var items = await queryable
            .Skip(checked(Math.Max(query.PageNumber, 0) * takeItems))
            .Take(takeItems)
            .ToListAsync(cancellationToken);

        return new() { Items = items, TotalCount = count };
    }
}
