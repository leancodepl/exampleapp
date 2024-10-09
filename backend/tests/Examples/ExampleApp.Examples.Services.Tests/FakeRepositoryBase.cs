using LeanCode.DomainModels.DataAccess;
using LeanCode.DomainModels.Model;

namespace ExampleApp.Examples.Services.Tests;

public abstract class FakeRepositoryBase<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : class, IAggregateRootWithoutOptimisticConcurrency<TId>
    where TId : struct
{
    protected Dictionary<TId, TEntity> Storage { get; } = [];

    public Task<TEntity?> FindAsync(TId id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Storage.TryGetValue(id, out var entity) ? entity : null);
    }

    public void Add(TEntity entity)
    {
        Storage[entity.Id] = entity;
    }

    public void Delete(TEntity entity)
    {
        Storage.Remove(entity.Id);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            Storage.Remove(entity.Id);
        }
    }

    public void Update(TEntity entity)
    {
        Storage[entity.Id] = entity;
    }
}
