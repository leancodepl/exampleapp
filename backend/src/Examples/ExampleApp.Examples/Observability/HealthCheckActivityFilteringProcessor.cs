using System.Diagnostics;
using OpenTelemetry;

namespace ExampleApp.Examples.Observability;

public class HealthCheckActivityFilteringProcessor : BaseProcessor<Activity>
{
    public override void OnEnd(Activity activity)
    {
        if (activity.Source.Name != "Microsoft.AspNetCore")
        {
            return;
        }

        if (
            activity.GetTagItem("http.route") is string route
            && route.StartsWith("/live")
            && activity.Status != ActivityStatusCode.Error
        )
        {
            activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
        }
    }
}
