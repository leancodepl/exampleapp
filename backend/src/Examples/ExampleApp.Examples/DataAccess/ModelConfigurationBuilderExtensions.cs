using LeanCode.DomainModels.EF;
using LeanCode.DomainModels.Ids;
using LeanCode.DomainModels.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExampleApp.Examples.DataAccess;

internal static class ModelConfigurationBuilderExtensions
{
    public static PropertiesConfigurationBuilder<TId> ConfigureId<TId>(
        this ModelConfigurationBuilder configurationBuilder
    )
        where TId : struct, IPrefixedTypedId<TId>
    {
        return configurationBuilder
            .Properties<TId>()
            .HaveColumnType("citext")
            .HaveConversion<PrefixedTypedIdConverter<TId>, PrefixedTypedIdComparer<TId>>();
    }

    public static PropertiesConfigurationBuilder<TId> ConfigureGuidId<TId>(
        this ModelConfigurationBuilder configurationBuilder
    )
        where TId : struct, IRawTypedId<Guid, TId>
    {
        return configurationBuilder
            .Properties<TId>()
            .HaveColumnType("uuid")
            .HaveConversion<RawTypedIdConverter<Guid, TId>, RawTypedIdComparer<Guid, TId>>();
    }

    public static void HasRowVersion<TEntity>(this EntityTypeBuilder<TEntity> e, bool makeOptimistic = true)
        where TEntity : class, IOptimisticConcurrency
    {
        e.Property<uint>("xmin").IsRowVersion();
        if (makeOptimistic)
        {
            e.IsOptimisticConcurrent(addRowVersion: false);
        }
    }
}
