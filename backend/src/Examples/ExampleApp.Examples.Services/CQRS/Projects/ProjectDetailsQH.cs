using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Domain.Projects;
using ExampleApp.Examples.Services.DataAccess;
using LeanCode.CQRS.Execution;
using LeanCode.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.CQRS.Projects;

public class ProjectDetailsQH : IQueryHandler<ProjectDetails, ProjectDetailsDTO?>
{
    private readonly ExamplesDbContext dbContext;

    public ProjectDetailsQH(ExamplesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<ProjectDetailsDTO?> ExecuteAsync(HttpContext context, ProjectDetails query)
    {
        if (!ProjectId.TryParse(query.Id, out var projectId))
        {
            return Task.FromResult<ProjectDetailsDTO?>(null);
        }

        return dbContext
            .Projects
            .Where(p => p.Id == projectId)
            .LeftJoin(
                dbContext.Employees,
                p => p.ProjectLeaderId,
                e => e.Id,
                (p, e) =>
                    new ProjectDetailsDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        ProjectLeaderId = p.ProjectLeaderId,
                        ProjectLeaderName = e!.Name,
                        Assignments = p.Assignments
                            .Select(a => new AssignmentDTO { Id = a.Id, Name = a.Name })
                            .ToList(),
                    }
            )
            .FirstOrDefaultAsync(context.RequestAborted);
    }
}
