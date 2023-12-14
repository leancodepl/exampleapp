#if Example
using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Domain.Projects;
using ExampleApp.Examples.Services.DataAccess;
using LeanCode.Contracts.Admin;
using LeanCode.CQRS.Execution;
using LeanCode.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.CQRS.Projects;

public class AllProjectsAdminQH : IQueryHandler<AllProjectsAdmin, AdminQueryResult<AdminProjectDTO>>
{
    private readonly ExamplesDbContext dbContext;

    public AllProjectsAdminQH(ExamplesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<AdminQueryResult<AdminProjectDTO>> ExecuteAsync(HttpContext context, AllProjectsAdmin query)
    {
        var projects = ApplyFilters(query, dbContext.Projects);
        projects = ApplySort(query, projects);
        var pageSize = Math.Min(query.PageSize, 10000);

        return new()
        {
            Total = await projects.CountAsync(context.RequestAborted),
            Items = await projects
                .Skip(query.Page * pageSize)
                .Take(pageSize)
                .Select(t => new AdminProjectDTO { Id = t.Id, Name = t.Name })
                .ToListAsync(context.RequestAborted),
        };
    }

    private static IQueryable<Project> ApplyFilters(AllProjectsAdmin query, IQueryable<Project> q)
    {
        if (!string.IsNullOrEmpty(query.NameFilter))
        {
            q = q.Where(r => r.Name.Contains(query.NameFilter));
        }

        return q;
    }

    private static IQueryable<Project> ApplySort(AllProjectsAdmin query, IQueryable<Project> q)
    {
        switch (query.SortBy)
        {
            case nameof(AdminProjectDTO.Name):
                q = q.OrderBy(t => t.Name, query.SortDescending == true).ThenBy(t => t.Id);
                break;
            default:
                q = q.OrderBy(t => t.Id);
                break;
        }

        return q;
    }
}
#endif
