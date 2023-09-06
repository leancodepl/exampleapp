using ExampleApp.Core.Domain.Projects;
using ExampleApp.Core.Domain.Projects.Events;
using LeanCode.DomainModels.DataAccess;
using MassTransit;

namespace ExampleApp.Core.Services.Processes.Projects;

public class DeleteProjectOnProjectNameChanged : IConsumer<ProjectNameChanged>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<DeleteProjectOnProjectNameChanged>();
    private readonly IRepository<Project, ProjectId> projects;

    public DeleteProjectOnProjectNameChanged(IRepository<Project, ProjectId> projects)
    {
        this.projects = projects;
    }

    public async Task Consume(ConsumeContext<ProjectNameChanged> context)
    {
        var domainEvent = context.Message;

        var project = await projects.FindAndEnsureExistsAsync(domainEvent.ProjectId, context.CancellationToken);

        projects.Delete(project);

        logger.Information("Project {ProjectId} deleted", project.Id);
    }
}
