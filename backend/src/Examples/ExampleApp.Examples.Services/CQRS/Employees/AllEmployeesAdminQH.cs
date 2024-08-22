using ExampleApp.Examples.Contracts.Employees;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Services.DataAccess;
using LeanCode.Contracts.Admin;
using LeanCode.CQRS.Execution;
using LeanCode.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.CQRS.Employees;

public class AllEmployeesAdminQH : IQueryHandler<AllEmployeesAdmin, AdminQueryResult<AdminEmployeeDTO>>
{
    private readonly ExamplesDbContext dbContext;

    public AllEmployeesAdminQH(ExamplesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<AdminQueryResult<AdminEmployeeDTO>> ExecuteAsync(HttpContext context, AllEmployeesAdmin query)
    {
        var employees = ApplyFilters(query, dbContext.Employees);
        employees = ApplySort(query, employees);
        var pageSize = Math.Min(query.PageSize, 10000);

        return new()
        {
            Total = await employees.CountAsync(context.RequestAborted),
            Items = await employees
                .Skip(query.Page * pageSize)
                .Take(pageSize)
                .Select(t => new AdminEmployeeDTO { Id = t.Id, Name = t.Name })
                .ToListAsync(context.RequestAborted),
        };
    }

    private static IQueryable<Employee> ApplyFilters(AllEmployeesAdmin query, IQueryable<Employee> q)
    {
        if (!string.IsNullOrEmpty(query.NameFilter))
        {
            q = q.Where(r => r.Name.Contains(query.NameFilter));
        }

        return q;
    }

    private static IQueryable<Employee> ApplySort(AllEmployeesAdmin query, IQueryable<Employee> q)
    {
        switch (query.SortBy)
        {
            case nameof(AdminEmployeeDTO.Name):
                q = q.OrderBy(t => t.Name, query.SortDescending == true).ThenBy(t => t.Id);
                break;
            default:
                q = q.OrderBy(t => t.Id);
                break;
        }

        return q;
    }
}
