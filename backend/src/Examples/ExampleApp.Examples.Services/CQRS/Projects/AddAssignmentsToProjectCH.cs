using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Domain.Projects;
using ExampleApp.Examples.Services.DataAccess;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.CQRS.Projects;

public class AddAssignmentsToProjectCV : AbstractValidator<AddAssignmentsToProject>
{
    public AddAssignmentsToProjectCV()
    {
        RuleFor(cmd => cmd.Assignments)
            .NotNull()
            .WithCode(AddAssignmentsToProject.ErrorCodes.AssignmentsCannotBeNull)
            .NotEmpty()
            .WithCode(AddAssignmentsToProject.ErrorCodes.AssignmentsCannotBeEmpty);

        RuleForEach(cmd => cmd.Assignments)
            .ChildRules(child => child.RuleFor(c => c).SetValidator(new AssignmentDTOValidator()));

        RuleFor(cmd => cmd.ProjectId)
            .Cascade(CascadeMode.Stop)
            .Must(ProjectId.IsValid)
            .WithCode(AddAssignmentsToProject.ErrorCodes.ProjectIdNotValid)
            .WithMessage("ProjectId has invalid format.")
            .CustomAsync(CheckProjectExistsAsync);
    }

    private async Task CheckProjectExistsAsync(
        string pid,
        ValidationContext<AddAssignmentsToProject> ctx,
        CancellationToken cancellationToken)
    {
        if (
            ProjectId.TryParse(pid, out var projectId)
            && !await ctx.GetService<ExamplesDbContext>().Projects.AnyAsync(p => p.Id == projectId, cancellationToken)
        )
        {
            ctx.AddValidationError(
                "A project with given Id does not exist.",
                AddAssignmentsToProject.ErrorCodes.ProjectDoesNotExist
            );
        }
    }
}

public class AssignmentDTOValidator : AbstractValidator<AssignmentWriteDTO>
{
    public AssignmentDTOValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .WithCode(AssignmentWriteDTO.ErrorCodes.NameCannotBeEmpty)
            .MaximumLength(500)
            .WithCode(AssignmentWriteDTO.ErrorCodes.NameTooLong);
    }
}

public class AddAssignmentsToProjectCH : ICommandHandler<AddAssignmentsToProject>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<AddAssignmentsToProjectCH>();

    private readonly IRepository<Project, ProjectId> projects;

    public AddAssignmentsToProjectCH(IRepository<Project, ProjectId> projects)
    {
        this.projects = projects;
    }

    public async Task ExecuteAsync(HttpContext context, AddAssignmentsToProject command)
    {
        var project = await projects.FindAndEnsureExistsAsync(
            ProjectId.Parse(command.ProjectId),
            context.RequestAborted
        );
        project.AddAssignments(command.Assignments.Select(a => a.Name));

        projects.Update(project);

        logger.Information(
            "{AssignmentCount} assignments added to project {ProjectId}.",
            command.Assignments.Count,
            project.Id
        );
    }
}
