using ExampleApp.Core.Domain.Projects;
using ExampleApp.Core.Domain.Projects.Events;
using LeanCode.DomainModels.DataAccess;
using MassTransit;

namespace ExampleApp.Core.Services.Processes.Projects;

public class ChangeProjectNameOnProjectCreated : IConsumer<ProjectCreated>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<ChangeProjectNameOnProjectCreated>();
    private readonly IRepository<Project, ProjectId> projects;

    public ChangeProjectNameOnProjectCreated(IRepository<Project, ProjectId> projects)
    {
        this.projects = projects;
    }

    public async Task Consume(ConsumeContext<ProjectCreated> context)
    {
        var domainEvent = context.Message;

        var project = await projects.FindAndEnsureExistsAsync(domainEvent.ProjectId, context.CancellationToken);

        project.ChangeName("Changed name from " + project.Name);

        projects.Update(project);

        logger.Information("Project {ProjectId} name changed", project.Id);
    }
}
