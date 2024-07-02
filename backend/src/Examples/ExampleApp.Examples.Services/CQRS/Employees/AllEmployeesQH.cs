using ExampleApp.Examples.Contracts.Employees;
using ExampleApp.Examples.Services.DataAccess;
using LeanCode.CQRS.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.CQRS.Employees;

public class AllEmployeesQH : IQueryHandler<AllEmployees, List<EmployeeDTO>>
{
    private readonly ExamplesDbContext dbContext;

    public AllEmployeesQH(ExamplesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<List<EmployeeDTO>> ExecuteAsync(HttpContext context, AllEmployees query)
    {
        return dbContext
            .Employees.OrderBy(e => e.Name)
            .Select(e => new EmployeeDTO
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
            })
            .ToListAsync(context.RequestAborted);
    }
}
