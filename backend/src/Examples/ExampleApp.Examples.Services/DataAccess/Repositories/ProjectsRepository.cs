#if Example
using ExampleApp.Examples.Domain.Projects;
using LeanCode.DomainModels.EF;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.DataAccess.Repositories;

public class ProjectsRepository : EFRepository<Project, ProjectId, ExamplesDbContext>
{
    public ProjectsRepository(ExamplesDbContext dbContext)
        : base(dbContext) { }

    public override Task<Project?> FindAsync(ProjectId id, CancellationToken cancellationToken = default)
    {
        return DbSet.AsTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<Project?> FindByAssignmentAsync(
        AssignmentId assignmentId,
        CancellationToken cancellationToken = default
    )
    {
        return DbSet
            .AsTracking()
            .FirstOrDefaultAsync(p => p.Assignments.Any(a => a.Id == assignmentId), cancellationToken)!;
    }
}
#endif
