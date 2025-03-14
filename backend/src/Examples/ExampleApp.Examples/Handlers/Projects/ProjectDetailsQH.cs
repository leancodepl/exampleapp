using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.DataAccess;
using ExampleApp.Examples.Domain.Projects;
using LeanCode.CQRS.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Handlers.Projects;

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
            .Projects.Where(p => p.Id == projectId)
            .Select(p => new ProjectDetailsDTO
            {
                Id = p.Id,
                Name = p.Name,
                Assignments = p
                    .Assignments.Select(a => new AssignmentDTO
                    {
                        Id = a.Id,
                        Name = a.Name,
                        AssignedEmployeeId = a.AssignedEmployeeId,
                    })
                    .ToList(),
            })
            .FirstOrDefaultAsync(context.RequestAborted);
    }
}
