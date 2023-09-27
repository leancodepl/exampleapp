using ExampleApp.Core.Domain.Projects;
using ExampleApp.Core.Domain.Projects.Events;
using LeanCode.DomainModels.DataAccess;
using MassTransit;

namespace ExampleApp.Core.Services.Processes.Projects;

public class IncreaseCounterOnProjectCounterIncreased : IConsumer<ProjectCounterIncreased>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<IncreaseCounterOnProjectCounterIncreased>();
    private readonly IRepository<Project, ProjectId> projects;

    public IncreaseCounterOnProjectCounterIncreased(IRepository<Project, ProjectId> projects)
    {
        this.projects = projects;
    }

    public async Task Consume(ConsumeContext<ProjectCounterIncreased> context)
    {
        var domainEvent = context.Message;

        var project = await projects.FindAndEnsureExistsAsync(domainEvent.ProjectId, context.CancellationToken);

        if (project.Name == "counter-test" && project.Counter < 120_000)
        {
            project.IncreaseCounter();

            projects.Update(project);

            logger.Information("Project {ProjectId} counter increased", project.Id);
        }
        else
        {
            logger.Information("Project {ProjectId} is not counter-test, ignoring", project.Id);
        }
    }
}
