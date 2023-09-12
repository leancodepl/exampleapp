using System.Diagnostics;
using LeanCode.OpenTelemetry;
using LeanCode.TimeProvider;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LeanCode.AuditLogs;

public class AuditLogsMiddleware<TDbContext>
    where TDbContext : DbContext
{
    private readonly RequestDelegate next;

    public AuditLogsMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, TDbContext dbContext, IBus bus)
    {
        await next(httpContext);

        var entitiesChanged = ChangedEntitiesExtractor.Extract(dbContext);
        var actorId = Activity.Current?.GetBaggageItem(IdentityTraceBaggageHelpers.UserIdKey);
        var actionName = httpContext.Request.Path.ToString();
        var now = Time.Now;

        await bus.Publish(
            new AuditLogMessage
            {
                EntitiesChanged = entitiesChanged,
                ActionName = actionName,
                DateOccurred = now,
                ActorId = actorId,
            },
            httpContext.RequestAborted
        );
    }
}
