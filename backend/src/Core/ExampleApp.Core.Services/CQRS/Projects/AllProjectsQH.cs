using ExampleApp.Core.Contracts.Projects;
using ExampleApp.Core.Services.DataAccess;
using LeanCode.CQRS.Execution;
using LeanCode.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Core.Services.CQRS.Projects;

public class AllProjectsQH : IQueryHandler<AllProjects, List<ProjectDTO>>
{
    private readonly CoreDbContext dbContext;

    public AllProjectsQH(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<List<ProjectDTO>> ExecuteAsync(HttpContext context, AllProjects query)
    {
        return dbContext
            .Projects
            .OrderBy(p => p.Name, query.SortByNameDescending)
            .Select(p => new ProjectDTO { Id = p.Id, Name = p.Name, })
            .ToListAsync(context.RequestAborted);
    }
}
