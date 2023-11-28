using ExampleApp.Core.Contracts.Employees;
using ExampleApp.Core.Services.DataAccess;
using LeanCode.CQRS.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Core.Services.CQRS.Employees;

public class AllEmployeesQH : IQueryHandler<AllEmployees, List<EmployeeDTO>>
{
    private readonly CoreDbContext dbContext;

    public AllEmployeesQH(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<List<EmployeeDTO>> ExecuteAsync(HttpContext context, AllEmployees query)
    {
        return dbContext
            .Employees
            .OrderBy(e => e.Name)
            .Select(
                e =>
                    new EmployeeDTO
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Email = e.Email,
                    }
            )
            .ToListAsync(context.RequestAborted);
    }
}
