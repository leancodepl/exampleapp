using ExampleApp.Core.Domain.Employees;
using LeanCode.DomainModels.EF;
using LeanCode.DomainModels.Model;
using LeanCode.TimeProvider;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Core.Services.DataAccess.Repositories;

public class EmployeesRepository : EFRepository<Employee, EmployeeId, CoreDbContext>
{
    public EmployeesRepository(CoreDbContext dbContext)
        : base(dbContext) { }

    public override void Add(Employee entity)
    {
        ((IOptimisticConcurrency)entity).DateModified = Time.NowWithOffset.UtcDateTime;
        DbSet.Add(entity);
    }

    public override void Delete(Employee entity)
    {
        ((IOptimisticConcurrency)entity).DateModified = Time.NowWithOffset.UtcDateTime;
        DbSet.Remove(entity);
    }

    public override void DeleteRange(IEnumerable<Employee> entities)
    {
        foreach (var item in entities)
        {
            ((IOptimisticConcurrency)item).DateModified = Time.NowWithOffset.UtcDateTime;
        }
        DbSet.RemoveRange(entities);
    }

    public override void Update(Employee entity)
    {
        ((IOptimisticConcurrency)entity).DateModified = Time.NowWithOffset.UtcDateTime;
    }

    public override Task<Employee?> FindAsync(EmployeeId id, CancellationToken cancellationToken = default)
    {
        return DbSet.AsTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken)!;
    }
}
