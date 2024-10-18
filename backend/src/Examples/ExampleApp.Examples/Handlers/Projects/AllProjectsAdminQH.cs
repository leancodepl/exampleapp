using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.DataAccess;
using ExampleApp.Examples.Domain.Projects;
using LeanCode.Contracts.Admin;
using LeanCode.CQRS.Execution;
using LeanCode.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Handlers.Projects;

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
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

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
        return q.ConditionalWhere(r => r.Name.Contains(query.NameFilter!), !string.IsNullOrEmpty(query.NameFilter));
    }

    private static IQueryable<Project> ApplySort(AllProjectsAdmin query, IQueryable<Project> q)
    {
        return query.SortBy switch
        {
            nameof(AdminProjectDTO.Name) => q.OrderBy(t => t.Name, query.SortDescending == true).ThenBy(t => t.Id),
            _ => q.OrderBy(t => t.Id),
        };
    }
}
