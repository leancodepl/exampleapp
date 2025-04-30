using System.Diagnostics;
using OpenTelemetry;

namespace ExampleApp.Examples.Observability;

public class MassTransitActivityFilteringProcessor : BaseProcessor<Activity>
{
    public override void OnStart(Activity activity)
    {
        if (activity.Source.Name is not MassTransit.Logging.DiagnosticHeaders.DefaultListenerName)
        {
            return;
        }

        if (activity.OperationName is "outbox process")
        {
            activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
        }
    }

    public override void OnEnd(Activity activity)
    {
        if (activity.Source.Name is not MassTransit.Logging.DiagnosticHeaders.DefaultListenerName)
        {
            return;
        }

        if (activity.OperationName is "outbox send")
        {
            if (activity.GetTagItem(MassTransit.Logging.DiagnosticHeaders.MessageTypes) is string messageTypes)
            {
                activity.DisplayName += $" ({messageTypes})";
            }
        }
    }
}
