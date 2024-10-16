using ExampleApp.Examples.Contracts.Employees;
using ExampleApp.Examples.Domain.Employees;
using ExampleApp.Examples.Services.DataAccess;
using LeanCode.Contracts.Admin;
using LeanCode.CQRS.Execution;
using LeanCode.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.Handlers.Employees;

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
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

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
        return q.ConditionalWhere(r => r.Name.Contains(query.NameFilter!), !string.IsNullOrEmpty(query.NameFilter));
    }

    private static IQueryable<Employee> ApplySort(AllEmployeesAdmin query, IQueryable<Employee> q)
    {
        return query.SortBy switch
        {
            nameof(AdminEmployeeDTO.Name) => q.OrderBy(t => t.Name, query.SortDescending == true).ThenBy(t => t.Id),
            _ => q.OrderBy(t => t.Id),
        };
    }
}
