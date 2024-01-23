using ExampleApp.Examples.Domain.Employees;
using LeanCode.DomainModels.EF;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Examples.Services.DataAccess.Repositories;

public class EmployeesRepository : EFRepository<Employee, EmployeeId, ExamplesDbContext>
{
    public EmployeesRepository(ExamplesDbContext dbContext)
        : base(dbContext) { }

    public override Task<Employee?> FindAsync(EmployeeId id, CancellationToken cancellationToken = default)
    {
        return DbSet.AsTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
