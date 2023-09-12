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
    private readonly IBus bus;

    public AuditLogsFilter(TDbContext dbContext, IBus bus)
    {
        this.dbContext = dbContext;
        this.bus = bus;
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

        await bus.Publish(
            new AuditLogMessage
            {
                EntitiesChanged = entitiesChanged,
                ActionName = actionName,
                DateOccurred = now,
                ActorId = actorId,
            },
            context.CancellationToken
        );
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
