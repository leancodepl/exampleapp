using ExampleApp.Core.Domain.Employees;
using LeanCode.DomainModels.EF;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Core.Services.DataAccess.Repositories;

public class EmployeesRepository : EFRepository<Employee, EmployeeId, CoreDbContext>
{
    public EmployeesRepository(CoreDbContext dbContext)
        : base(dbContext) { }

    public override Task<Employee?> FindAsync(EmployeeId id, CancellationToken cancellationToken = default)
    {
        return DbSet.AsTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken)!;
    }
}
