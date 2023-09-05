using System.Diagnostics;
using LeanCode.OpenTelemetry;
using LeanCode.TimeProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
        var changeTracker = dbContext.ChangeTracker;

        await next(httpContext);

        var entitiesChanged = dbContext.ChangeTracker
            .Entries()
            .Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached)
            .Select(
                e =>
                    new
                    {
                        Ids = e.Metadata
                            .FindPrimaryKey()
                            ?.Properties.Select(p => p.PropertyInfo?.GetMethod?.Invoke(e.Entity, null)),
                        Type = e.Metadata.ClrType,
                        Changes = e.DebugView.LongView
                    }
            )
            .ToList();

        foreach (var entityChanged in entitiesChanged)
        {
            await auditLogStorage.StoreEventAsync(
                entityChanged.Type.ToString(),
                entityChanged.Ids?.Select(i => i?.ToString() ?? throw new NullReferenceException(nameof(i)))
                    ?? throw new NullReferenceException(nameof(entityChanged.Ids)),
                httpContext.Request.Path.ToString().Split('.').LastOrDefault(),
                Time.Now,
                Activity.Current?.GetBaggageItem(IdentityTraceBaggageHelpers.UserIdKey),
                entityChanged.Changes,
                httpContext.RequestAborted
            );
        }
    }
}
