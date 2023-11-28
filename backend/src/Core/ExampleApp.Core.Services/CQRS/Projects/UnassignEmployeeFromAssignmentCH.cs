using ExampleApp.Core.Contracts.Projects;
using ExampleApp.Core.Domain.Employees;
using ExampleApp.Core.Domain.Projects;
using ExampleApp.Core.Services.DataAccess;
using ExampleApp.Core.Services.DataAccess.Repositories;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Core.Services.CQRS.Projects;

public class UnassignEmployeeFromAssignmentCV : AbstractValidator<UnassignEmployeeFromAssignment>
{
    public UnassignEmployeeFromAssignmentCV()
    {
        RuleFor(cmd => cmd.AssignmentId)
            .Cascade(CascadeMode.Stop)
            .Must(AssignmentId.IsValid)
            .WithCode(UnassignEmployeeFromAssignment.ErrorCodes.AssignmentIdNotValid)
            .WithMessage("AssignmentId has invalid format.")
            .CustomAsync(CheckProjectWithAssignmentExistsAsync);
    }

    private static async Task CheckProjectWithAssignmentExistsAsync(
        string aid,
        ValidationContext<UnassignEmployeeFromAssignment> ctx,
        CancellationToken ct
    )
    {
        if (
            AssignmentId.TryParse(aid, out var assignmentId)
            && !await ctx.GetService<CoreDbContext>()
                .Projects
                .SelectMany(p => p.Assignments)
                .AnyAsync(a => a.Id == assignmentId, ct)
        )
        {
            ctx.AddValidationError(
                "A project with given assignment ID does not exist.",
                UnassignEmployeeFromAssignment.ErrorCodes.ProjectWithAssignmentDoesNotExist
            );
        }
    }
}

public class UnassignEmployeeFromAssignmentCH : ICommandHandler<UnassignEmployeeFromAssignment>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<UnassignEmployeeFromAssignmentCH>();

    private readonly ProjectsRepository projects;

    public UnassignEmployeeFromAssignmentCH(ProjectsRepository projects)
    {
        this.projects = projects;
    }

    public async Task ExecuteAsync(HttpContext context, UnassignEmployeeFromAssignment command)
    {
        var assignmentId = AssignmentId.Parse(command.AssignmentId);

        var project = await projects.FindByAssignmentAsync(
            AssignmentId.Parse(command.AssignmentId),
            context.RequestAborted
        );

        project!.UnassignEmployeeFromAssignment(assignmentId);

        projects.Update(project);

        logger.Information(
            "Employee unassigned from assignment {AssignmentId} in project {ProjectId}",
            assignmentId,
            project.Id
        );
    }
}
