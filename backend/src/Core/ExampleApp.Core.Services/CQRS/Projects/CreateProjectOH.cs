using ExampleApp.Core.Contracts.Projects;
using ExampleApp.Core.Domain.Projects;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using Microsoft.AspNetCore.Http;

namespace ExampleApp.Core.Services.CQRS.Projects;

public class CreateProjectOH : IOperationHandler<CreateProjectOperation, bool>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<CreateProjectOH>();

    private readonly IRepository<Project, ProjectId> projects;

    public CreateProjectOH(IRepository<Project, ProjectId> projects)
    {
        this.projects = projects;
    }

    public Task<bool> ExecuteAsync(HttpContext context, CreateProjectOperation command)
    {
        var project = Project.Create(command.Name);
        projects.Add(project);

        logger.Information("Project {ProjectId} added", project.Id);

        return Task.FromResult(true);
    }
}
