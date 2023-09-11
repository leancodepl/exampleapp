using System.Diagnostics;
using LeanCode.CQRS.MassTransitRelay;
using LeanCode.OpenTelemetry;
using LeanCode.TimeProvider;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace LeanCode.AuditLogs;

public class AuditLogsFilter<TDbContext, TConsumer, TMessage> : IFilter<ConsumerConsumeContext<TConsumer, TMessage>>
    where TDbContext : DbContext
    where TConsumer : class
    where TMessage : class
{
    private readonly TDbContext dbContext;
    private readonly IAuditLogStorage auditLogStorage;

    public AuditLogsFilter(TDbContext dbContext, IAuditLogStorage auditLogStorage)
    {
        this.dbContext = dbContext;
        this.auditLogStorage = auditLogStorage;
    }

    public void Probe(ProbeContext context) { }

    public async Task Send(
        ConsumerConsumeContext<TConsumer, TMessage> context,
        IPipe<ConsumerConsumeContext<TConsumer, TMessage>> next
    )
    {
        await next.Send(context);

        var entitiesChanged = ChangedEntitiesExtractor.Extract(dbContext);
        var actorId = Activity.Current?.GetBaggageItem(IdentityTraceBaggageHelpers.UserIdKey);
        var actionName = context.Consumer.ToString()!;
        var now = Time.Now;

        await auditLogStorage.StoreEventAsync(entitiesChanged, actionName, now, actorId, context.CancellationToken);
    }
}

public static class EventsPublisherFilterExtensions
{
    public static void UseAuditLogs<TDbContext>(this IConsumePipeConfigurator configurator, IServiceProvider provider)
        where TDbContext : DbContext
    {
        configurator.UseTypedConsumeFilter<Observer<TDbContext>>(provider);
    }

    private sealed class Observer<TDbContext> : ScopedTypedConsumerConsumePipeSpecificationObserver
        where TDbContext : DbContext
    {
        public override void ConsumerMessageConfigured<TConsumer, TMessage>(
            IConsumerMessageConfigurator<TConsumer, TMessage> configurator
        ) =>
            configurator.AddConsumerScopedFilter<AuditLogsFilter<TDbContext, TConsumer, TMessage>, TConsumer, TMessage>(
                Provider
            );
    }
}
