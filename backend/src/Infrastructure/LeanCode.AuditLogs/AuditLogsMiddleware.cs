using System.Diagnostics;
using LeanCode.OpenTelemetry;
using LeanCode.TimeProvider;
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

    public async Task InvokeAsync(HttpContext httpContext, TDbContext dbContext, IAuditLogStorage auditLogStorage)
    {
        await next(httpContext);

        var entitiesChanged = ChangedEntitiesExtractor<TDbContext>.Extract(dbContext);
        var actorId = Activity.Current?.GetBaggageItem(IdentityTraceBaggageHelpers.UserIdKey);
        var actionName = httpContext.Request.Path.ToString();
        var now = Time.Now;

        var storeLogs = entitiesChanged.Select(
            entityChanged =>
                auditLogStorage.StoreEventAsync(entityChanged, actionName, now, actorId, httpContext.RequestAborted)
        );

        await Task.WhenAll(storeLogs);
    }
}
