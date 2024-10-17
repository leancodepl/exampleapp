using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Services.DataAccess;
using LeanCode.CQRS.Execution;
using LeanCode.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.Handlers.Projects;

public class AllProjectsQH : IQueryHandler<AllProjects, List<ProjectDTO>>
{
    private readonly ExamplesDbContext dbContext;

    public AllProjectsQH(ExamplesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<List<ProjectDTO>> ExecuteAsync(HttpContext context, AllProjects query)
    {
        return dbContext
            .Projects.OrderBy(p => p.Name, query.SortByNameDescending)
            .Select(p => new ProjectDTO { Id = p.Id, Name = p.Name })
            .ToListAsync(context.RequestAborted);
    }
}
