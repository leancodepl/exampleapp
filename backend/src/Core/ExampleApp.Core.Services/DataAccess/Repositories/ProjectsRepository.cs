using ExampleApp.Core.Domain.Projects;
using LeanCode.DomainModels.EF;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Core.Services.DataAccess.Repositories;

public class ProjectsRepository : EFRepository<Project, ProjectId, CoreDbContext>
{
    public ProjectsRepository(CoreDbContext dbContext)
        : base(dbContext) { }

    public override Task<Project?> FindAsync(ProjectId id, CancellationToken cancellationToken = default)
    {
        return DbSet.AsTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken)!;
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
