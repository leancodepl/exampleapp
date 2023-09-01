using ExampleApp.Core.Contracts.Employees;
using ExampleApp.Core.Domain.Employees;
using ExampleApp.Core.Services.DataAccess;
using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LeanCode.DomainModels.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Core.Services.CQRS.Employees;

public class CreateEmployeeCV : AbstractValidator<CreateEmployee>
{
    public CreateEmployeeCV()
    {
        RuleFor(cmd => cmd.Name)
            .NotEmpty()
            .WithCode(CreateEmployee.ErrorCodes.NameCannotBeEmpty)
            .MaximumLength(500)
            .WithCode(CreateEmployee.ErrorCodes.NameTooLong);

        RuleFor(cmd => cmd.Email)
            .EmailAddress()
            .WithCode(CreateEmployee.ErrorCodes.EmailInvalid)
            .CustomAsync(CheckEmployeeExistsAsync);
    }

    private static async Task CheckEmployeeExistsAsync(
        string email,
        ValidationContext<CreateEmployee> ctx,
        CancellationToken ct
    )
    {
        if (await ctx.GetService<CoreDbContext>().Employees.AnyAsync(e => e.Email == email, ct))
        {
            ctx.AddValidationError(
                "An employee with such email already exists.",
                CreateEmployee.ErrorCodes.EmailIsNotUnique
            );
        }
    }
}

public class CreateEmployeeCH : ICommandHandler<CreateEmployee>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<CreateEmployeeCH>();

    private readonly IRepository<Employee, EmployeeId> employees;

    public CreateEmployeeCH(IRepository<Employee, EmployeeId> employees)
    {
        this.employees = employees;
    }

    public Task ExecuteAsync(HttpContext context, CreateEmployee command)
    {
        var employee = Employee.Create(command.Name, command.Email);
        employees.Add(employee);

        logger.Information("Employee {EmployeeId} added", employee.Id);

        return Task.CompletedTask;
    }
}
