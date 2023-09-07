# LeanCode.AuditLogs

Package dedicated to auditing changes of domain objects modified during execution of command/operation/event handlers.

Package uses EntityFramework ChangeTracker in order to detect and log changes. Each log consists of:
- object type
- object identifier
- handler name
- date of the change
- actor executing changes
- object changes

## Dependencies

`LeanCode.AuditLogs` depend on `IdentityTraceAttributesMiddleware` from  `LeanCode.OpenTelemetry` - if this middleware
is not configured, then the `ActorId` will be always set to `null`.

## Configuration

The package do not require extra work from the user other than initial configuration. In order to collect audit logs
from all handlers there are three things to configure.

### AuditLogStorage

AuditLogs collectors require `IAuditLogStorage` to be registered in the DI container.

```csharp
public override void ConfigureServices(IServiceCollection services)
{
    // some other code

    services.AddTransient<IAuditLogStorage, StubAuditLogStorage>();

    // some other code
}
```

Currently there are couple of ready made implementations of `IAuditLogStorage`:
- StubAuditLogStorage - sample implementation that logs audit information using `Serilog` logger.

If you want to use some other store for your data feel free to implement `IAuditLogStorage` on your own.

### Endpoints

In order to collect audit logs you need to add `Audit<TDbContext>()` middleware to execution pipeline. The `TDbContext`
argument is a `DbContext` where we want to audit the changes.

Example configuration using AuditLogs looks as follows:

```csharp
protected override void ConfigureApp(IApplicationBuilder app)
{
    // some other code

    app.UseEndpoints(
        endpoints => endpoints.MapRemoteCqrs(
            "/api",
            cqrs =>
            {
                cqrs.Commands = c =>
                    c.CQRSTrace()
                        .Secure()
                        .Validate()
                        .CommitTransaction<CoreDbContext>()
                        .PublishEvents()
                        .Audit<CoreDbContext>();
                cqrs.Queries = c => c.CQRSTrace().Secure();
                cqrs.Operations = c =>
                    c.CQRSTrace()
                        .Secure()
                        .CommitTransaction<CoreDbContext>()
                        .PublishEvents()
                        .Audit<CoreDbContext>();
            }
        )
    );

    // some other code
}
```

⚠️ Bear in mind, that the order here makes difference. If you don't want to collect changes in the MT inbox/outbox
tables, then you should configure `Audit<TDbContext>()` middleware **after** the `PublishEvents()` middleware.

### Consumers

In order to add audit logs to event handling the only thing you need to do is to add `.UseAuditLogs<TDbContext>(sp)`
to the consumer configuration.

```csharp
protected override void ConfigureConsumer(
    IReceiveEndpointConfigurator endpointConfigurator,
    IConsumerConfigurator<TConsumer> consumerConfigurator
)
{
    endpointConfigurator.UseRetry(
        r => r.Immediate(1).Incremental(3, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5))
    );
    endpointConfigurator.UseEntityFrameworkOutbox<CoreDbContext>(serviceProvider);
    endpointConfigurator.UseDomainEventsPublishing(serviceProvider);
    endpointConfigurator.UseAuditLogs<CoreDbContext>(serviceProvider);
}
```

⚠️ Bear in mind, that the order here makes difference. If you don't want to collect changes in the MT inbox/outbox
tables, then you should configure `UseAuditLogs<TDbContext>(sp)` filter **after** the `UseDomainEventsPublishing(sp)`
filter.
