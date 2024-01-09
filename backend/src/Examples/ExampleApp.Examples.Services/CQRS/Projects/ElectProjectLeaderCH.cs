using ExampleApp.Examples.Contracts.Projects;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Domain.Projects;
using ExampleApp.Examples.Services.DataAccess;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.CQRS.Projects;

public class ElectProjectLeaderCV : AbstractValidator<ElectProjectLeader>
{
    public ElectProjectLeaderCV()
    {
        RuleFor(cmd => cmd.ProjectId)
            .Cascade(CascadeMode.Stop)
            .MustAsync(CheckProjectExistsAsync)
            .WithCode(ElectProjectLeader.ErrorCodes.ProjectDoesNotExist)
            .WithMessage("A project with given Id does not exist.");

        RuleFor(cmd => cmd.EmployeeId)
            .Cascade(CascadeMode.Stop)
            .MustAsync(CheckEmployeeExistsAsync)
            .WithCode(ElectProjectLeader.ErrorCodes.EmployeeDoesNotExist)
            .WithMessage("An employee with given Id does not exist.");

        RuleFor(cmd => cmd)
            .MustAsync(CheckProjectHasAssignedEmployeeAsync)
            .WithCode(ElectProjectLeader.ErrorCodes.EmployeeMustBeAssignedToAnyAssignment)
            .WithMessage("Employee with no assignments cannot be the project leader.");
    }

    private static async Task<bool> CheckEmployeeExistsAsync(
        ElectProjectLeader cmd,
        string eid,
        ValidationContext<ElectProjectLeader> ctx,
        CancellationToken ct
    )
    {
        return EmployeeId.TryParse(eid, out var employeeId)
            && await ctx.GetService<ExamplesDbContext>().Employees.AnyAsync(e => e.Id == employeeId, ct);
    }

    private static async Task<bool> CheckProjectExistsAsync(
        ElectProjectLeader cmd,
        string pid,
        ValidationContext<ElectProjectLeader> ctx,
        CancellationToken ct
    )
    {
        return ProjectId.TryParse(pid, out var projectId)
            && await ctx.GetService<ExamplesDbContext>().Projects.AnyAsync(p => p.Id == projectId, ct);
    }

    private static async Task<bool> CheckProjectHasAssignedEmployeeAsync(
        ElectProjectLeader cmd,
        ElectProjectLeader _,
        ValidationContext<ElectProjectLeader> ctx,
        CancellationToken ct
    )
    {
        return ProjectId.TryParse(cmd.ProjectId, out var projectId)
            && EmployeeId.TryParse(cmd.EmployeeId, out var employeeId)
            && await ctx.GetService<ExamplesDbContext>()
                .Projects
                .Where(p => p.Id == projectId)
                .SelectMany(p => p.Assignments)
                .AnyAsync(a => a.AssignedEmployeeId == employeeId, ct);
    }
}

public class ElectProjectLeaderCH : ICommandHandler<ElectProjectLeader>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<ElectProjectLeaderCH>();

    private readonly IRepository<Project, ProjectId> projects;

    public ElectProjectLeaderCH(IRepository<Project, ProjectId> projects)
    {
        this.projects = projects;
    }

    public async Task ExecuteAsync(HttpContext context, ElectProjectLeader command)
    {
        var project = await projects.FindAndEnsureExistsAsync(
            ProjectId.Parse(command.ProjectId),
            context.RequestAborted
        );

        project.ElectProjectLeader(EmployeeId.Parse(command.EmployeeId));

        projects.Update(project);

        logger.Information(
            "Employee {EmployeeId} elected as project {ProjectId} leader",
            command.EmployeeId,
            command.ProjectId
        );
    }
}
