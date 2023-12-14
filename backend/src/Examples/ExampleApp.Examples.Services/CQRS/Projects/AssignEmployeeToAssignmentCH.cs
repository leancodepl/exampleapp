#if Example
using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using ExampleApp.Examples.Services.DataAccess;
using ExampleApp.Examples.Services.DataAccess.Repositories;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.CQRS.Projects;

public class AssignEmployeeToAssignmentCV : AbstractValidator<AssignEmployeeToAssignment>
{
    public AssignEmployeeToAssignmentCV()
    {
        RuleFor(cmd => cmd.AssignmentId)
            .Cascade(CascadeMode.Stop)
            .Must(AssignmentId.IsValid)
            .WithCode(AssignEmployeeToAssignment.ErrorCodes.AssignmentIdNotValid)
            .WithMessage("AssignmentId has invalid format.")
            .CustomAsync(CheckProjectWithAssignmentExistsAsync);

        RuleFor(cmd => cmd.EmployeeId)
            .Cascade(CascadeMode.Stop)
            .Must(EmployeeId.IsValid)
            .WithCode(AssignEmployeeToAssignment.ErrorCodes.EmployeeIdNotValid)
            .WithMessage("EmployeeId has invalid format.")
            .CustomAsync(CheckEmployeeExistsAsync);
    }

    private static async Task CheckProjectWithAssignmentExistsAsync(
        string aid,
        ValidationContext<AssignEmployeeToAssignment> ctx,
        CancellationToken ct
    )
    {
        if (
            AssignmentId.TryParse(aid, out var assignmentId)
            && !await ctx.GetService<ExamplesDbContext>()
                .Projects
                .SelectMany(p => p.Assignments)
                .AnyAsync(a => a.Id == assignmentId, ct)
        )
        {
            ctx.AddValidationError(
                "A project with given assignment ID does not exist.",
                AssignEmployeeToAssignment.ErrorCodes.ProjectWithAssignmentDoesNotExist
            );
        }
    }

    private static async Task CheckEmployeeExistsAsync(
        string pid,
        ValidationContext<AssignEmployeeToAssignment> ctx,
        CancellationToken ct
    )
    {
        if (
            EmployeeId.TryParse(pid, out var employeeId)
            && !await ctx.GetService<ExamplesDbContext>().Employees.AnyAsync(p => p.Id == employeeId, ct)
        )
        {
            ctx.AddValidationError(
                "A employee with given ID does not exist.",
                AssignEmployeeToAssignment.ErrorCodes.EmployeeDoesNotExist
            );
        }
    }
}

public class AssignEmployeeToAssignmentCH : ICommandHandler<AssignEmployeeToAssignment>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<AssignEmployeeToAssignmentCH>();

    private readonly ProjectsRepository projects;

    public AssignEmployeeToAssignmentCH(ProjectsRepository projects)
    {
        this.projects = projects;
    }

    public async Task ExecuteAsync(HttpContext context, AssignEmployeeToAssignment command)
    {
        var assignmentId = AssignmentId.Parse(command.AssignmentId);
        var employeeId = EmployeeId.Parse(command.EmployeeId);

        var project = await projects.FindByAssignmentAsync(
            AssignmentId.Parse(command.AssignmentId),
            context.RequestAborted
        );

        project!.AssignEmployeeToAssignment(assignmentId, employeeId);

        projects.Update(project);

        logger.Information(
            "Employee {EmployeeId} assigned to assignment {AssignmentId} in project {ProjectId}",
            employeeId,
            assignmentId,
            project.Id
        );
    }
}
#endif
