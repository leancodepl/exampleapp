using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Services.DataAccess;
using LeanCode.CQRS.Execution;
using LeanCode.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.CQRS.Projects;

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
            .Projects
            .OrderBy(p => p.Name, query.SortByNameDescending)
            .LeftJoin(
                dbContext.Employees,
                p => p.ProjectLeaderId,
                e => e.Id,
                (p, e) =>
                    new ProjectDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ProjectLeaderId = p.ProjectLeaderId,
                        ProjectLeaderName = e!.Name,
                    }
            )
            .ToListAsync(context.RequestAborted);
    }
}
